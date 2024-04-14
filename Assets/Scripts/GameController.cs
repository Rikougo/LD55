using System.Collections.Generic;
using Summoning.Combination;
using Summoning.Monster;
using Summoning.UI;
using UnityEngine;

namespace Summoning
{
    public enum GameState
    {
        PLAYING,
        PAUSE
    }

    public class GameController : MonoBehaviour
    {
        [Header("Monsters")] [SerializeField] private MonsterController m_monsterController;
        [SerializeField] private float m_manaRegenRate = 10.0f;
        [SerializeField] private float m_manaCost = 10.0f;

        [SerializeField] private MonsterDropAsset m_monsterDropAsset;
        [SerializeField] private MonsterDrop m_monsterDropPrefab;

        [Header("Summons")] [SerializeField] private CombineController m_combineController;
        [SerializeField] private CombinationController m_combinationController;
        [SerializeField] private InventoryController m_inventoryController;

        [Header("Tower")] [SerializeField] private Transform m_gunPosition;
        [SerializeField] private ProjectileComponent m_projectilePrefab;
        [SerializeField] private int m_towerDamage = 25;
        [SerializeField] private float m_baseTowerHealth = 10;
        [SerializeField] private SpriteRenderer m_towerRenderer;
        [SerializeField] private Sprite m_baseTowerSprite;
        [SerializeField] private List<Sprite> m_towerSprites;
        [SerializeField] private GunComponent m_gunComponent;
        [SerializeField] private float m_attackRate = 1.5f;

        private GameState m_gameState;

        // Tower
        private float m_lastTimeShot;
        private float m_currentManaRegenRate;
        private float m_monsterMana;

        // Monsters
        private List<CombinedMonster> m_monsters;
        private Queue<CombinationPart> m_monstersToSpawn;

        // Summons
        private Dictionary<CombinationPart, int> m_partInventory;
        private List<CombinedMonster> m_summons;
        private float m_towerHealth;

        private void Awake()
        {
            OnReset();
        }

        private void Start()
        {
            m_combineController.Combined += SpawnSummon;
        }

        private void Update()
        {
            switch (m_gameState)
            {
                case GameState.PLAYING:
                    TickGame();
                    break;
                case GameState.PAUSE:
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D p_other)
        {
            if (p_other.CompareTag("Enemy"))
            {
                DamageTower(1);
                if (p_other.TryGetComponent(out CombinedMonster l_monster))
                {
                    l_monster.Kill();
                    if (m_monsters.Contains(l_monster))
                    {
                        m_monsters.Remove(l_monster);
                    }
                }
            }
        }

        private void OnReset()
        {
            m_monstersToSpawn = new Queue<CombinationPart>();
            m_monstersToSpawn.Enqueue(CombinationPart.EARTH);
            m_monstersToSpawn.Enqueue(CombinationPart.WATER);

            m_currentManaRegenRate = m_manaRegenRate;
            m_monsterMana = m_manaCost;

            m_monsters = new List<CombinedMonster>();

            m_lastTimeShot = Time.time;

            m_partInventory = new Dictionary<CombinationPart, int>();
            m_summons = new List<CombinedMonster>();
            m_towerHealth = m_baseTowerHealth;

            m_inventoryController.OnReset();

            m_towerRenderer.sprite = m_baseTowerSprite;
        }

        public void DamageTower(int p_amount)
        {
            m_towerHealth -= p_amount;

            if (m_towerHealth <= 0)
            {
                OnGameOver();
                return;
            }

            float l_healthPercent = (float)m_towerHealth / (float)m_baseTowerHealth;
            int l_healthIndex = (int)Mathf.Ceil(l_healthPercent * m_towerSprites.Count);
            m_towerRenderer.sprite = m_towerSprites[^l_healthIndex];
        }

        private void TickGame()
        {
            ScaleDifficulty();
            ProcessPickup();

            m_monsterMana += m_currentManaRegenRate * Time.deltaTime;
            if (m_monsterMana > m_manaCost)
            {
                SpawnMonster();
                m_monsterMana -= m_manaCost;
            }

            if (m_monsters.Count > 0)
            {
                foreach (var l_combinedMonster in m_monsters)
                {
                    if (l_combinedMonster.CurrentState == CombinedMonster.MonsterState.ATTACK) break;
                    
                    var l_transform = l_combinedMonster.transform;
                    l_transform.position = l_transform.position + l_transform.right * Time.deltaTime;
                }

                ShootIfNeeded();
            }

            if (m_summons.Count > 0)
            {
                foreach (var l_combinedMonster in m_summons)
                {
                    if (l_combinedMonster.CurrentState == CombinedMonster.MonsterState.ATTACK) break;
                    
                    var l_transform = l_combinedMonster.transform;
                    l_transform.position = l_transform.position + l_transform.right * Time.deltaTime;
                }
            }
            
            // Proceed fight 
            if (m_summons.Count > 0 && m_monsters.Count > 0)
            {
                CombinedMonster l_summon = m_summons[0];
                CombinedMonster l_monster = m_monsters[0];

                if ((l_summon.transform.position - l_monster.transform.position).sqrMagnitude < 2.0f)
                {
                    l_summon.CurrentState = CombinedMonster.MonsterState.ATTACK;
                    l_monster.CurrentState = CombinedMonster.MonsterState.ATTACK;

                    l_summon.TickAttack(l_monster);
                    l_monster.TickAttack(l_summon);
                }
                else
                {
                    l_summon.CurrentState = CombinedMonster.MonsterState.MOVE;
                    l_summon.CurrentState = CombinedMonster.MonsterState.MOVE;
                }
            }
        }

        private void ScaleDifficulty()
        {
            m_currentManaRegenRate += Time.deltaTime * 0.05f;
        }

        private void ProcessPickup()
        {
            var l_colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                                      1 << LayerMask.NameToLayer("Drop"));
            if (Input.GetMouseButtonDown(0) && l_colliders != null && l_colliders.Length > 0)
            {
                foreach (var l_collider2D in l_colliders)
                {
                    var l_drop = l_collider2D.gameObject.GetComponent<MonsterDrop>();
                    if (l_drop != null)
                    {
                        m_partInventory.TryAdd(l_drop.Part, 0);
                        m_partInventory[l_drop.Part] += 1;

                        m_inventoryController.UpdateDisplay(m_partInventory);

                        l_drop.Collect();
                    }
                }
            }
        }

        private void SpawnMonster()
        {
            if (m_monstersToSpawn.Count == 0) return;

            var l_partToSpawn = m_monstersToSpawn.Dequeue();
            var l_monster = m_monsterController.SummonMonster(l_partToSpawn);

            if (l_monster != null)
            {
                m_monsters.Add(l_monster);
                if (m_monsters.Count == 1) l_monster.Died += OnFirstMonsterDied;
                m_monstersToSpawn.Enqueue(MonsterAsset.ExistingMonsters
                                              [Random.Range(0, MonsterAsset.ExistingMonsters.Length)]);
            }
            else
            {
                Debug.LogWarning("Could not spawn monster");
            }
        }

        private void OnFirstMonsterDied()
        {
            var l_monster = m_monsters[0];
            m_monsters.RemoveAt(0);
            SpawnDrop(l_monster.Part, l_monster.transform.position + new Vector3(0.0f, 0.25f, 0.0f));

            l_monster.Died -= OnFirstMonsterDied;

            if (m_monsters.Count > 0)
            {
                m_monsters[0].Died += OnFirstMonsterDied;
            }

            if (m_summons.Count > 0)
            {
                m_summons[0].CurrentState = CombinedMonster.MonsterState.MOVE;
            }
        }

        private void SpawnDrop(CombinationPart p_part, Vector3 p_position)
        {
            if (m_monsterDropAsset.TryGetDrop(p_part, out var l_sprite))
            {
                var l_instance = Instantiate(m_monsterDropPrefab, p_position, Quaternion.identity);
                l_instance.Init(l_sprite, p_part);
            }
        }

        private void ShootIfNeeded()
        {
            var l_closestMonster = m_monsters[0].transform;
            var l_closestPos = l_closestMonster.position;
            l_closestPos.y += 1.0f;
            if ((l_closestPos - m_gunPosition.position).sqrMagnitude < 300.0f)
                if (Time.time - m_lastTimeShot > m_attackRate)
                {
                    var l_projectile = Instantiate(m_projectilePrefab, m_gunPosition);
                    l_projectile.Direction = l_closestPos - m_gunPosition.position;
                    l_projectile.Damage = m_towerDamage;
                    l_projectile.Target = l_closestMonster.GetComponent<CombinedMonster>();
                    m_lastTimeShot = Time.time;
                    m_gunComponent.Fire();
                }
        }

        private void SpawnSummon(CombinationPart p_arm, CombinationPart p_body)
        {
            m_partInventory[p_arm] -= 1;
            m_partInventory[p_body] -= 1;
            m_inventoryController.UpdateDisplay(m_partInventory);
            var l_instance = m_combinationController.SummonCombination(p_arm, p_body);

            if (l_instance != null)
            {
                m_summons.Add(l_instance);
                if (m_summons.Count == 1) l_instance.Died += OnFirstSummonDied;
            }
        }

        private void OnFirstSummonDied()
        {
            CombinedMonster l_first = m_summons[0];
            m_summons.RemoveAt(0);

            // ON PLANT DEATH HEAL ALL MONSTERS
            if (l_first.BodyType == CombinationPart.PLANT)
            {
                foreach (var l_combinedMonster in m_summons)
                {
                    l_combinedMonster.Heal(30);
                }
            }

            if (m_summons.Count > 0)
            {
                m_summons[0].Died += this.OnFirstSummonDied;
            }

            if (m_monsters.Count > 0)
            {
                m_monsters[0].CurrentState = CombinedMonster.MonsterState.MOVE;
            }
        }

        private void Cleanup()
        {
            if (m_monsters.Count > 0) m_monsters[0].Died -= OnFirstMonsterDied;
            foreach (var l_combinedMonster in m_monsters) Destroy(l_combinedMonster.gameObject);
            m_monsters = null;
            if (m_summons.Count > 0) m_summons[0].Died -= OnFirstSummonDied;
            foreach (var l_combinedMonster in m_summons) Destroy(l_combinedMonster.gameObject);
            m_summons = null;
            foreach (var l_monsterDrop in FindObjectsByType<MonsterDrop>(FindObjectsInactive.Include,
                                                                         FindObjectsSortMode.None))
                Destroy(l_monsterDrop.gameObject);
        }

        private void OnGameOver()
        {
            Cleanup();
            OnReset();
        }
    }
}