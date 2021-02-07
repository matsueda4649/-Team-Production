using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static ExtensionInput;

namespace ItemSystem
{
    public class DragObject : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        #region 変数宣言

        /// <summary>
        /// アイコン
        /// </summary>
        [SerializeField] Sprite m_iconSprite = default;

        private SpriteRenderer m_renderer;

        /// <summary>
        /// Mainカメラ
        /// </summary>
        private Camera m_camera;

        /// <summary>
        /// Drag開始時のコールバック
        /// </summary>
        private Action m_onBeginDrag;

        /// <summary>
        /// Drag時のコールバック
        /// </summary>
        private Action m_drag;

        /// <summary>
        /// スクリーン座標
        /// </summary>
        private Vector3 m_screenPoint;

        /// <summary>
        /// ワールド座標上のカーソルと、対象の位置の差
        /// </summary>
        private Vector3 m_offset;
        
        #endregion

        #region メソッド

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        protected virtual void Initialize()
        {
            m_camera = Camera.main;
            this.SetComponent(ref m_renderer);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="onBeginDrag">Drag開始時のコールバック</param>
        /// <param name="drag">Drag時のコールバック</param>
        public void SetCallback(Action onBeginDrag, Action drag)
        {
            m_onBeginDrag = onBeginDrag;
            m_drag = drag;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var thisPosition = transform.position;

            m_screenPoint = m_camera.WorldToScreenPoint(thisPosition);

            var mouse = GetInputPosition();
            m_offset = thisPosition - GetInputWorldPosition(m_camera, m_screenPoint.z);

            m_onBeginDrag?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            m_drag?.Invoke();

            Vector3 currentPosition = GetInputWorldPosition(m_camera, m_screenPoint.z) + m_offset;
            transform.position = currentPosition;
        }

        /// <summary>
        /// Click時
        /// </summary>
        public void PointerClick()
        {
            m_renderer.ChangeSprite(m_iconSprite);
        }

        #endregion
    }
}