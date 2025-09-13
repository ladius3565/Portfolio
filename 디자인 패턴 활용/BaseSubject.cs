using System.Collections.Generic;

//관찰자 클래스에서 상속받을 인터페이스
public interface IObserver
{
    void OnNotify();
}
//관찰자 클래스에 받을 이벤트가 여러개 일 때 인터페이스
public interface IObserver<T>
{
    void OnNotify(T data);
}

//주체 클래스가 상속받을 클래스
public class BaseSubject
{
    protected List<IObserver> observers = new();
    //관찰자 클래스에서 이벤트를 받은 뒤 구독을 취소하는 경우를 대비하여 구성
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
            //만약 Notify() 함수가 실행 중 구독을 취소하는 경우 나머지 리스트 실행에 문제가 없도록 하기 위해 사용
            index -= 1;
        }
    }
}
//주체 클래스에서 보낼 이벤트가 여러개 일 때 클래스
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