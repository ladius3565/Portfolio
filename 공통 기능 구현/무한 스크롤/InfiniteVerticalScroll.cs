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
        //�ش� ��ũ�ѿ� ���Ǵ� ������
        [SerializeField] protected GameObject prefab;
        [Space(2)]
        //��ũ���� �����¿� ��� �Ÿ�
        [SerializeField] protected RectOffset padding;
        [Space(4)]
        //�� ���Ե� ���� ��� �Ÿ�
        [SerializeField] protected float spacing;
        [Space(2)]
        [Tooltip("������ ���� �� ��ũ�� �������� ����")]
        //��ũ�ѿ� �ʿ��� ���Ժ��� ���� �� ��ũ�� �������� ����
        [SerializeField] protected bool isEnable;
        #endregion

        public List<T> datas { get; protected set; }
        protected LinkedList<ScrollSlot<T>> slots = new();

        //��ũ��UI���� �������� ������ ����ϱ� ���� ���
        protected Rect visibleRect;
        //��ũ���� �����̰� �ִ� ���� ����� ���� ������ ��ũ�� ��ġ�� ����
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

        //��� ������ ���̰� ������ �� ����
        protected float slotHeight;
        //��ũ�ѿ� �ʿ��� �ּ� ����
        protected int minSlot;
        //��ũ�ѿ� �ʿ��� �ִ� ����
        protected int maxSlot;

        //Start �Լ��� ȣ�� �Ǿ����� üũ
        protected bool isInit;
        //Start �Լ� ȣ�� ����(�ʱ�ȭ �۾� ����) ȣ��Ǿ�� �� �̺�Ʈ
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
        //StartAction�� ����ϴ� �Լ�
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

        //������ �Է� �Լ�
        public void InitData(List<T> data)
        {            
            datas = data;
            CheckEnable();
            if (isInit) InitView();
        }
        //������ �ʱ�ȭ �Լ�
        public void InitData()
        {
            if (datas is null) return;
            datas.Clear();
            CheckEnable();
            InitView();
        }
        //������ �Է��ϸ鼭 ������� ��ũ�� �̵�
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
        //������ �Է��ϸ鼭 Ư�� ��ġ�� ��ũ�� �̵�
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

        //������ �߰�
        public virtual void AddData(T data)
        {
            datas.Add(data);
            InitView();
        }
        //������ ����
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
        //��� ���� ������ ������Ʈ
        public void UpdateSlot()
        {
            foreach (var item in slots)
            {
                UpdateSlotIdx(item, item.idx);
            }
        }

        //��ũ���� �ֻ������ �̵�
        public void ScrollTop()
        {
            if (!isInit) return;
            MoveScrollTop();
        }
        //��ũ���� ���ϴ����� �̵�
        public void ScrollBottom() 
        {
            if (!isInit) return;
            MoveScrollBottom(); 
        }
        //��ũ���� Ư�� ��ġ�� �̵�
        public void ScrollIndex(int index)
        {
            if (!isInit) return;
            if (index <= 0 || datas.Count <= minSlot)
                MoveScrollTop();
            else if (index >= datas.Count - minSlot) 
                MoveScrollBottom();
            else MoveScrollIndex(index);
        }

        //��ũ���� ��� ������ �ʱ�ȭ
        protected void InitView()
        {
            UpdateContentSize();
            UpdateVisibleRect();
            UpdateView();
        }
        //���Ե��� ��ġ�� ���ġ
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

        //������ ���� ����
        protected virtual float OnHeight(int idx) { return slotHeight; }

        //��ũ�� �ÿ� ȣ��Ǵ� �Լ�
        protected virtual void OnScroll(Vector2 pos)
        {
            UpdateVisibleRect();
            
            ScrollSlot(visibleRect.y > prevScrollY);

            prevScrollY = visibleRect.y;
        }

        //���� ������ ����� �°� Content������ ����(��ũ�� �� ������ �缳��)
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
        //VisibleRect�� ���� �ʱ�ȭ
        protected void UpdateVisibleRect()
        {
            visibleRect.x = Scroll.content.anchoredPosition.x;
            visibleRect.y = -Scroll.content.anchoredPosition.y;

            visibleRect.width = Scroll.viewport.rect.width;
            visibleRect.height = Scroll.viewport.rect.height;
        }

        //��ũ�� �Ǵ� ���⿡ ���� ���Ե� ��ġ�� �̵�
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

        //������ �ʿ��� �����ͷ� ������Ʈ
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
        //���� �⺻�� �Ǵ� ���������� ���� ����
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
        //������ ũ�⸦ �̿��� �ش� ���Կ� �ʿ��� �ּ�, �ִ� ũ�� ���
        protected virtual void CalcScroll() //�ּ� ũ�� �������� ���
        {
            var maxHeight = Mathf.Abs(Scroll.viewport.rect.height);

            var maxCount = (int)(maxHeight / (OnHeight(0) + spacing));

            var voidHeight = maxHeight - OnHeight(0) * maxCount - spacing * (maxCount - 1);

            minSlot = maxCount;
            var plus = voidHeight - spacing * 2 <= 0 ? 0 : 1;
            maxSlot = minSlot + plus;
        }
        //�ִ� ���� ������ �°� ���� ����
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
        //�����Ͱ� �ּ� �������� �۴ٸ� ��ũ�� �������� ���
        private void CheckEnable()
        {
            if (isEnable)
                Scroll.enabled = datas.Count > minSlot;
        }

        //��ũ���� �ֻ������ �̵�
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
        //��ũ���� ���ϴ����� �̵�
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
        //��ũ���� ���ϴ� ��ġ�� �̵�
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