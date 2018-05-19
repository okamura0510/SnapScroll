using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace SnapScroll
{
    /// <summary>
    /// スナップスクロールビュー
    /// </summary>
    [AddComponentMenu("UI/SnapScrollView", 100)]
    public class SnapScrollView : ScrollRect
    {
        /// <summary>
        /// 現在ページ(0〜)
        /// </summary>
        [SerializeField] int page;
        /// <summary>
        /// 最大ページ(0〜)
        /// </summary>
        [SerializeField] int maxPage;
        /// <summary>
        /// ページサイズ
        /// </summary>
        [SerializeField] float pageSize;
        /// <summary>
        /// スクロール可能な判定距離(フリックのしやすさ)
        /// </summary>
        [SerializeField] float scrollableDistance = 2;
        /// <summary>
        /// アンカーポジション移動Tween
        /// </summary>
        [SerializeField] TweenAnchorPosition tween;
        /// <summary>
        /// 現在のドラッグポジション
        /// </summary>
        Vector3 dragPos;
        /// <summary>
        /// 前回のドラッグポジション
        /// </summary>
        Vector3 prevDragPos;

        /// <summary>
        /// 現在ページ(0〜)
        /// </summary>
        public int Page { get { return page; } set { page = value; } }
        /// <summary>
        /// 最大ページ(0〜)
        /// </summary>
        public int MaxPage { get { return maxPage; } set { maxPage = value; } }
        /// <summary>
        /// ページサイズ
        /// </summary>
        public float PageSize { get { return pageSize; } set { pageSize = value; } }
        /// <summary>
        /// スクロール可能な判定距離(フリックのしやすさ)
        /// </summary>
        public float ScrollableDistance { get { return scrollableDistance; } set { scrollableDistance = value; } }
        /// <summary>
        /// アンカーポジション移動Tween
        /// </summary>
        public TweenAnchorPosition Tween { get { return tween; } }
        /// <summary>
        /// ページ変化イベント
        /// </summary>
        public event Action OnPageChanged;

        void Update()
        {
            if(tween.IsRunning) tween.Update();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
        
            tween.Stop();
            dragPos     = content.position;
            prevDragPos = Vector3.zero;
        }
        
        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
        
            prevDragPos = dragPos;
            dragPos     = content.position;
        }
        
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
        
            StopMovement();
        
            // 「ページ内移動量」「最終フレームドラッグ量(フリック量)」でスクロール可能か判定
            var pageDx       = -pageSize * page - content.localPosition.x;
            var dragDx       = prevDragPos.x    - content.position.x;
            var pageDistance = Mathf.Abs(pageDx);
            var dragDistance = Math.Abs(dragDx);
            var isScrollable = false;
            var isRight      = false;
            if(pageDistance >= pageSize / 2)
            {
                // ページ半分以上動かしていたら強制的に次ページへ
                isScrollable = true;
                isRight      = (pageDx >= 0);
            }
            else if(dragDistance >= scrollableDistance)
            {
                // スクロール可能な距離以上動かしていたら次ページへ
                isScrollable = true;
                isRight      = (dragDx >= 0);
            }
        
            if(isScrollable)
            {
                // 最大・最小ページを超えないようにページ更新
                if((isRight && page < maxPage) || (!isRight && page >= 1))
                {
                    page += isRight ? 1 : -1;
                }
            }
        
            RefreshPage();
        }
        
        /// <summary>
        /// ページリフレッシュ
        /// </summary>
        /// <param name="isPlayAnimation">アニメーションを再生させるか</param>
        public void RefreshPage(bool isPlayAnimation = true)
        {
            var movePos = content.anchoredPosition;
            movePos.x   = -pageSize * page;
            if(isPlayAnimation)
            {
                tween.Run(content, movePos);
            }
            else
            {
                content.anchoredPosition = movePos;
            }
            
            if(OnPageChanged != null) OnPageChanged();
        }
    }
}