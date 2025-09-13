using System.Collections.Generic;

//������ Ŭ�������� ��ӹ��� �������̽�
public interface IObserver
{
    void OnNotify();
}
//������ Ŭ������ ���� �̺�Ʈ�� ������ �� �� �������̽�
public interface IObserver<T>
{
    void OnNotify(T data);
}

//��ü Ŭ������ ��ӹ��� Ŭ����
public class BaseSubject
{
    protected List<IObserver> observers = new();
    //������ Ŭ�������� �̺�Ʈ�� ���� �� ������ ����ϴ� ��츦 ����Ͽ� ����
    protected int index;

    public void Notify()
    {        
        for (index = 0; index <observers.Count; index++)
        {
            observers[index].OnNotify();
        }
    }

    public void Register(IObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }
    public void Remove(IObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
            //���� Notify() �Լ��� ���� �� ������ ����ϴ� ��� ������ ����Ʈ ���࿡ ������ ������ �ϱ� ���� ���
            index -= 1;
        }
    }
}
//��ü Ŭ�������� ���� �̺�Ʈ�� ������ �� �� Ŭ����
public class BaseSubject<T>
{
    protected List<IObserver<T>> observers = new();
    protected int index;

    public void Notify(T data)
    {
        for (index = 0; index < observers.Count; index++)
        {
            observers[index].OnNotify(data);
        }
    }

    public void Register(IObserver<T> observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }
    public void Remove(IObserver<T> observer)
    {
        if (observers.Contains(observer))
            observers.Remove(observer);
    }
}