using ControllerSystem;
using DG.Tweening;
using UnityEngine;
using static ExtensionDOTween;

/// <summary>
/// 落下
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Drop : MonoBehaviour
{
    #region 変数宣言

    [SerializeField] Collider2D[] m_collider = new Collider2D[2];

    private Rigidbody2D m_rb;

    private BasePlayerController m_playerController = null;

    private Transform m_transform;

    private Tween m_tween = null;

    /// <summary>
    /// アニメーションフラグ
    /// </summary>
    private bool m_punching = false;

    #endregion

    #region メソッド

    private void Start()
    {
        this.SetComponent(ref m_rb);
        m_transform = this.transform;
        m_rb.SetBodyType(RigidbodyType2D.Kinematic);
        m_collider[0].enabled = true;
        m_collider[1].enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_punching)
        {
            collision.SetComponent(ref m_playerController);
            PunchMotion();
        }
        else
        {
            collision.SetComponent(ref m_playerController);
            if (!m_playerController.IsNull())
            {
                m_playerController.DeathMotion();
            }
        }
    }

    /// <summary>
    /// 落下前の準備モーション
    /// </summary>
    private void PunchMotion()
    {
        if (!m_playerController.IsNull())
        {
            m_collider[0].enabled = false;
            m_punching = true;

            m_tween = DOPunchPositionX(m_transform, 0.5f, () => {

                m_playerController = null;
                m_collider[1].enabled = true;
                m_rb.SetBodyType(RigidbodyType2D.Dynamic);
            });
        }
    }

    private void OnDisable()
    {
        Kill(m_tween);
    }

    #endregion
}
