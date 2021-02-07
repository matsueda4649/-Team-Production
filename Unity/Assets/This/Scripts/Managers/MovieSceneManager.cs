using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace ManagerSystem
{
    /// <summary>
    /// 初期時のムービーを管理
    /// </summary>
    public class MovieSceneManager : BaseScene
    {
        private static readonly string SceneName = "MovieScene";

        #region 変数宣言

        /// <summary>
        /// 目標座標の親オブジェクト
        /// </summary>
        [SerializeField] RectTransform m_targetParent = default;

        /// <summary>
        /// 目的座標
        /// </summary>
        private RectTransform[] m_tagetArray;

        [SerializeField] RectTransform m_camera = default;

        #region Character

        /// <summary>
        /// プレイヤー
        /// </summary>
        [SerializeField] RectTransform m_player = default;

        private Animator m_playerAnimator = null;

        /// <summary>
        /// 姫
        /// </summary>
        [SerializeField] RectTransform m_princess = default;

        private Animator m_princessAnimator = null;

        /// <summary>
        /// 敵
        /// </summary>
        [SerializeField] RectTransform m_enemy = default;

        #endregion

        private IList<Tween> m_tweenList = new List<Tween>(2);

        #endregion

        #region メソッド

        public override string GetSceneName()
        {
            return SceneName;
        }

        public override async UniTask Initialize()
        {
            var length = m_targetParent.childCount;
            m_tagetArray = new RectTransform[length];
            for(int i = 0; i < length; ++i)
            {
                var child = m_targetParent.GetChild(i);
                m_tagetArray[i] = child.GetComponent<RectTransform>();           
            }

            m_player.SetComponent(ref m_playerAnimator);
            m_princess.SetComponent(ref m_princessAnimator);

            AudioManager.Instance.PlayBgm(AudioBgm.Opening_1);

            await UniTask.Delay(DefTime.DELAY_TIME);
        }

        public override async UniTask StartScene()
        {
            await UniTask.Delay(DefTime.DELAY_TIME);

            StartFirstMove();
        }

        /// <summary>
        /// 主人公と姫が一緒に歩く
        /// </summary>
        private void StartFirstMove()
        {
            m_playerAnimator.SetBool("Walk", true);
            m_princessAnimator.SetBool("Walk", true);

            var startPos = m_player.localPosition;
            m_player.DOLocalMoveX(m_tagetArray[0].localPosition.x, 5f).OnComplete(()=> {

                m_playerAnimator.SetBool("Walk", false);
                m_princessAnimator.SetBool("Walk", false);
                AudioManager.Instance.StopBgm();
                StartSecondMove();

            }).SetEase(Ease.Linear);
        }

        /// <summary>
        /// 姫を誘拐する
        /// </summary>
        private void StartSecondMove()
        {
            m_enemy.DOLocalMoveY(m_tagetArray[1].transform.localPosition.y, 0.5f).
                SetEase(Ease.InQuart)
                .OnComplete(()=> {

                    AudioManager.Instance.PlayBgm(AudioBgm.Opening_2);
                    m_princess.SetParent(null);
                    m_princess.SetParent(m_enemy);
                    m_princess.transform.SetLocalRotation(new Vector3(0f,180f,0f));

                    StartThirdMove();
                });
        }

        /// <summary>
        /// 敵がプレイヤーを吹き飛ばす
        /// </summary>
        private void StartThirdMove()
        {
            m_player.DOLocalJump(m_tagetArray[2].localPosition, 0.5f, 3, 1)
                .SetEase(Ease.Linear)
                .OnComplete(()=>{

                    m_camera.SetParent(null);
                    m_camera.SetParent(m_player);

                    m_tweenList.Add(m_enemy.DOLocalMove(m_tagetArray[3].localPosition, 8f));
                    DOVirtual.DelayedCall(
                        0.8f, // 1秒待機させる
                        () =>{

                            m_playerAnimator.SetBool("Run", true);
                            m_tweenList.Add(m_player.DOLocalMoveX(m_tagetArray[4].localPosition.x, 10f)
                            .SetEase(Ease.Linear));

                        DOVirtual.DelayedCall(
                            5f,
                            async ()=>{
                                var saveData = SaveManager.Instance.GetSaveData;
                                saveData.onNewGame = false;
                                SaveManager.Save(saveData);
                                await GameSceneManager.Instance.LoadScene("StageSelectScene");
                            });
                        });
                });
        }

        private void OnDisable()
        {
            ExtensionDOTween.Kill(m_tweenList);
        }

        #endregion
    }
}