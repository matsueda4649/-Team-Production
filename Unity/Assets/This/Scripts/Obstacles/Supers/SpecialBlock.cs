using ControllerSystem;
using UnityEngine;

/// <summary>
/// 特殊なブロック
/// </summary>
public abstract class SpecialBlock : MonoBehaviour
{
    #region 変数宣言

    [SerializeField] Collider2D m_trigger = default;

    /// <summary>
    /// トリガーを取得
    /// </summary>
    public Collider2D Trigger { get => m_trigger; }

    private SpriteRenderer m_renderer;

    /// <summary>
    /// SpriteRendererを取得
    /// </summary>
    public SpriteRenderer GetRenderer { get => m_renderer; }

    #endregion

    #region メソッド

    protected virtual void Awake()
    {
        this.SetComponent(ref m_renderer);
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected abstract void Initialize();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        var playerController = collision.GetComponent<BasePlayerController>();
        if (!playerController.IsNull())
        {
            TriggerWithPlayer(playerController);
        }
    }

    /// <summary>
    /// プレイヤーとの衝突処理
    /// </summary>
    /// <param name="playerController"></param>
    protected virtual void TriggerWithPlayer(BasePlayerController playerController)
    {
        m_trigger.enabled = false;
        m_renderer.color = Color.gray;
    }

    #endregion
}