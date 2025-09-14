using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Scroll
{
    [RequireComponent(typeof(ScrollRect))]
    public abstract class InfiniteVerticalScroll<T> : MonoBehaviour
    {        
        #region Inspector
        //해당 스크롤에 사용되는 프리팹
        [SerializeField] protected GameObject prefab;
        [Space(2)]
        //스크롤의 상하좌우 띄울 거리
        [SerializeField] protected RectOffset padding;
        [Space(4)]
        //각 슬롯들 끼리 띄울 거리
        [SerializeField] protected float spacing;
        [Space(2)]
        [Tooltip("수량이 적을 때 스크롤 고정할지 여부")]
        //스크롤에 필요한 슬롯보다 적을 때 스크롤 고정할지 여부
        [SerializeField] protected bool isEnable;
        #endregion

        public List<T> datas { get; protected set; }
        protected LinkedList<ScrollSlot<T>> slots = new();

        //스크롤UI에서 보여지는 영역을 계산하기 위해 사용
        protected Rect visibleRect;
        //스크롤이 움직이고 있는 방향 계산을 위해 마지막 스크롤 위치를 저장
        private float prevScrollY;

        private RectTransform rect;
        public RectTransform Rect
        {
            get
            {
                if (rect == null) rect = GetComponent<RectTransform>();
                return rect;
            }
        }

        private ScrollRect scroll;
        public ScrollRect Scroll
        {
            get
            {
                if (scroll == null) scroll = GetComponent<ScrollRect>();
                return scroll;
            }
        }

        //모든 슬롯의 높이가 동일할 때 리턴
        protected float slotHeight;
        //스크롤에 필요한 최소 개수
        protected int minSlot;
        //스크롤에 필요한 최대 개수
        protected int maxSlot;

        //Start 함수가 호출 되었는지 체크
        protected bool isInit;
        //Start 함수 호출 이후(초기화 작업 이후) 호출되어야 할 이벤트
        protected UnityAction StartAction;

        private void Start()
        {            
            Scroll.onValueChanged.AddListener(OnScroll);            
            if (prefab.TryGetComponent<ScrollSlot<T>>(out var slot))
            {
                slots.AddLast(slot);
                slotHeight = slot.Height;
            }            
            CalcScroll();
            BatchSlot();
            if (datas is null)
                datas = new();
            InitView();
            CheckEnable();
            isInit = true;
            StartAction?.Invoke();
        }
        //StartAction에 등록하는 함수
        public void Action(UnityAction action)
        {
            if (isInit) action();
            else
                StartAction = action;
        }
        public void AddAction(UnityAction action)
        {
            if (isInit) action();
            else
                StartAction += action;
        }

        //데이터 입력 함수
        public void InitData(List<T> data)
        {            
            datas = data;
            CheckEnable();
            if (isInit) InitView();
        }
        //데이터 초기화 함수
        public void InitData()
        {
            if (datas is null) return;
            datas.Clear();
            CheckEnable();
            InitView();
        }
        //데이터 입력하면서 상단으로 스크롤 이동
        public void InitTop(List<T> data)
        {
            datas = data;
            CheckEnable();
            if (isInit)
            {
                UpdateContentSize();
                MoveScrollTop();
            }
        }
        //데이터 입력하면서 특정 위치로 스크롤 이동
        public void InitIdx(List<T> data, int idx)
        {
            datas = data;
            CheckEnable();
            if (isInit)
            {
                UpdateContentSize();
                ScrollIndex(idx);
            }
        }

        //데이터 추가
        public virtual void AddData(T data)
        {
            datas.Add(data);
            InitView();
        }
        //데이터 제거
        public virtual void RemoveData(T data)
        {
            datas.Remove(data);
            InitView();
        }       
        public virtual void RemoveData(int idx)
        {
            datas.RemoveAt(idx);
            InitView();
        }
        //모든 슬롯 데이터 업데이트
        public void UpdateSlot()
        {
            foreach (var item in slots)
            {
                UpdateSlotIdx(item, item.idx);
            }
        }

        //스크롤을 최상단으로 이동
        public void ScrollTop()
        {
            if (!isInit) return;
            MoveScrollTop();
        }
        //스크롤을 최하단으로 이동
        public void ScrollBottom() 
        {
            if (!isInit) return;
            MoveScrollBottom(); 
        }
        //스크롤을 특정 위치로 이동
        public void ScrollIndex(int index)
        {
            if (!isInit) return;
            if (index <= 0 || datas.Count <= minSlot)
                MoveScrollTop();
            else if (index >= datas.Count - minSlot) 
                MoveScrollBottom();
            else MoveScrollIndex(index);
        }

        //스크롤의 모든 정보를 초기화
        protected void InitView()
        {
            UpdateContentSize();
            UpdateVisibleRect();
            UpdateView();
        }
        //슬롯들의 위치를 재배치
        protected void UpdateView()
        {            
            var node = slots.First;
            if (node.Value.idx < 0) node.Value.idx = 0;
            else if (node.Value.idx >= datas.Count)
            {
                var idx = datas.Count - maxSlot - 1;
                if (idx < 0) idx = 0;
                node.Value.idx = idx;
            }
            UpdateSlotIdx(node.Value, node.Value.idx);

            var height = (float)padding.top;
            for (var i = 0; i < node.Value.idx; i++)
            {
                height += OnHeight(i) + spacing;
            }
            var top = new Vector2(padding.left, -height);
            node.Value.Top = top;
            node = node.Next;

            while (node is not null)
            {
                var idx = node.Previous.Value.idx + 1;
                top = node.Previous.Value.Bottom + new Vector2(0f, -spacing);
                UpdateSlotIdx(node.Value, idx);
                node.Value.Top = top;
                node = node.Next;
            }
        }

        //슬롯의 높이 리턴
        protected virtual float OnHeight(int idx) { return slotHeight; }

        //스크롤 시에 호출되는 함수
        protected virtual void OnScroll(Vector2 pos)
        {
            UpdateVisibleRect();
            
            ScrollSlot(visibleRect.y > prevScrollY);

            prevScrollY = visibleRect.y;
        }

        //현재 데이터 사이즈에 맞게 Content사이즈 변경(스크롤 될 범위를 재설정)
        protected void UpdateContentSize()
        {
            var height = 0f;
            for (var i = 0; i < datas.Count; i++)
            {
                height += OnHeight(i);
                if (i > 0) height += spacing;
            }

            var size = Scroll.content.sizeDelta;
            size.y = padding.top + height + padding.bottom;
            Scroll.content.sizeDelta = size;
        }
        //VisibleRect의 범위 초기화
        protected void UpdateVisibleRect()
        {
            visibleRect.x = Scroll.content.anchoredPosition.x;
            visibleRect.y = -Scroll.content.anchoredPosition.y;

            visibleRect.width = Scroll.viewport.rect.width;
            visibleRect.height = Scroll.viewport.rect.height;
        }

        //스크롤 되는 방향에 맞춰 슬롯들 위치를 이동
        private void ScrollSlot(bool up)
        {
            if (slots.Count == 0) return;
            if (up)
            {
                var last = slots.Last.Value;                
                while (last.Top.y < visibleRect.y - visibleRect.height)
                {
                    var first = slots.First.Value;
                    var bottom = first.Top + new Vector2(0f, spacing);

                    UpdateSlotIdx(last, first.idx - 1);
                    last.Bottom = bottom;

                    slots.AddFirst(last);
                    slots.RemoveLast();
                    last = slots.Last.Value;
                }
            }
            else
            {
                var first = slots.First.Value;                
                while (first.Bottom.y > visibleRect.y)
                {
                    var last = slots.Last.Value;
                    var top = last.Bottom + new Vector2(0f, -spacing);
                    UpdateSlotIdx(first, last.idx + 1);
                    first.Top = top;

                    slots.AddLast(first);
                    slots.RemoveFirst();
                    first = slots.First.Value;
                }
            }
        }

        //슬롯을 필요한 데이터로 업데이트
        protected virtual void UpdateSlotIdx(ScrollSlot<T> slot, int idx)
        {
            slot.idx = idx;
            if (slot.idx >= 0 && slot.idx <= datas.Count - 1)
            {
                slot.gameObject.SetActive(true);
                slot.Initialize(datas[idx]);
                slot.Height = OnHeight(idx);
            }
            else slot.gameObject.SetActive(false);
        }
        //가장 기본이 되는 프리팹으로 슬롯 생성
        protected virtual ScrollSlot<T> CreateSlot(int idx)
        {
            var obj = Instantiate(prefab, Scroll.content);
            if (obj.TryGetComponent<ScrollSlot<T>>(out var slot))
            {
                slot.idx = idx;
                return slot;
            }

            return null;
        }
        //슬롯의 크기를 이용해 해당 슬롯에 필요한 최소, 최대 크기 계산
        protected virtual void CalcScroll() //최소 크기 기준으로 계산
        {
            var maxHeight = Mathf.Abs(Scroll.viewport.rect.height);

            var maxCount = (int)(maxHeight / (OnHeight(0) + spacing));

            var voidHeight = maxHeight - OnHeight(0) * maxCount - spacing * (maxCount - 1);

            minSlot = maxCount;
            var plus = voidHeight - spacing * 2 <= 0 ? 0 : 1;
            maxSlot = minSlot + plus;
        }
        //최대 슬롯 수량에 맞게 슬롯 생성
        protected virtual void BatchSlot()
        {
            var top = new Vector2(padding.left, -padding.top);
            for (var i = 0; i < maxSlot; i++)
            {
                var slot = CreateSlot(i);
                slot.Top = top;
                top = slot.Bottom + new Vector2(0f, -spacing);
                slots.AddLast(slot);
            }
        }
        //데이터가 최소 수량보다 작다면 스크롤 고정할지 계산
        private void CheckEnable()
        {
            if (isEnable)
                Scroll.enabled = datas.Count > minSlot;
        }

        //스크롤을 최상단으로 이동
        private void MoveScrollTop()
        {
            Scroll.content.anchoredPosition = Vector2.zero;
            Scroll.StopMovement();
            UpdateVisibleRect();
            var top = new Vector2(padding.left, -padding.top);
            var idx = 0;
            foreach (var item in slots)
            {
                UpdateSlotIdx(item, idx++);
                item.Top = top;
                top = item.Bottom + new Vector2(0, -spacing);
            }
        }
        //스크롤을 최하단으로 이동
        private void MoveScrollBottom()
        {
            Scroll.content.anchoredPosition = new Vector2(0, Scroll.content.rect.height - Scroll.viewport.rect.height);
            Scroll.StopMovement();
            UpdateVisibleRect();
            var idx = datas.Count - 1;
            var bottom = new Vector2(padding.left, padding.bottom - Scroll.content.rect.height);
            foreach (var item in slots.Reverse())
            {
                if (idx < 0) break;
                UpdateSlotIdx(item, idx--);
                item.Bottom = bottom;
                bottom = item.Top + new Vector2(0f, spacing);
            }
        }
        //스크롤을 원하는 위치로 이동
        private void MoveScrollIndex(int index)
        {
            var height = (float)padding.top;
            for (var i = 0; i < index; i++)
            {
                height += OnHeight(i) + spacing;
            }
            Scroll.content.anchoredPosition = new Vector2(0, height);
            Scroll.StopMovement();
            UpdateVisibleRect();
            index -= 1;
            var node = slots.First;
            height -= OnHeight(index) + spacing;
            var top = new Vector2(padding.left, -height);
            if (index >= 0)
            {
                UpdateSlotIdx(node.Value, index++);
                node.Value.Top = top;
                top = node.Value.Bottom + new Vector2(0f, -spacing);
                node = node.Next;
            }
            while (node != null)
            {
                UpdateSlotIdx(node.Value, index++);
                node.Value.Top = top;
                top = node.Value.Bottom + new Vector2(0f, -spacing);
                node = node.Next;
            }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
        public bool active
        {
            get { return gameObject.activeSelf; }
        }
    }
}