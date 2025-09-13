# 개요
[돌아가기](https://github.com/ladius3565/Portfolio/blob/main/README.md)



회사에 근무하며 UI 콘텐츠 개발할 때 UI에서 사용하던 구조에 대한 코드입니다.

주로 MVP 패턴과 옵저버 패턴을 활용하였습니다.

![UML 이미지](https://github.com/ladius3565/Portfolio/blob/main/%EB%94%94%EC%9E%90%EC%9D%B8%20%ED%8C%A8%ED%84%B4%20%ED%99%9C%EC%9A%A9/Image/DesignPattern_UML.png)

위 UML 이미지 처럼 구조를 잡아 사용하였습니다.

또한 Model에서 입력 이벤트에 의해 정보 업데이트가 이루어져 UI에서 갱신이 되어야 할 때 전달해주는 방식은 옵저버 패턴을 활용하였습니다.

옵저버 패턴을 사용함으로 여러 UI에서 동시에 정보 업데이트에 대한 호출을 받을 수 있게 되는데 

![Observer 예시](https://github.com/ladius3565/Portfolio/blob/main/%EB%94%94%EC%9E%90%EC%9D%B8%20%ED%8C%A8%ED%84%B4%20%ED%99%9C%EC%9A%A9/Image/ObserverPattern_EX.png)

이미지 처럼 스킬 리스트UI 위에 스킬 정보UI가 있는 경우 두 UI 모두 변경된 정보에 대한 업데이트가 필요하게 되는데 옵저버 패턴을 통해 UI가 열릴 때 Model에 구독하고 닫힐 때는 취소하는 구조로 만들어 열려있는 UI만 업데이트 되도록 처리합니다.


![MVP 예시](https://github.com/ladius3565/Portfolio/blob/main/%EB%94%94%EC%9E%90%EC%9D%B8%20%ED%8C%A8%ED%84%B4%20%ED%99%9C%EC%9A%A9/Image/MVPPattern_EX.png)

최종적으로 위의 이미지와 같은 구조로 개별 UI가 구현되게 됩니다.
View에서는 UI의 이미지, 텍스트, 이펙트 와 같은 정보만을 보유하며 Presenter에서 정보를 전달받아 UI 정보를 업데이트 합니다.

Presenter에서는 View가 보유한 UI에서 입력을 받은 뒤 그에 대한 처리를 Model에 요청하고 옵저버 패턴을 통해 변경된 정보를 받아와 View로 전달합니다.

Model은 Presenter를 통해 실질적인 데이터 처리를 맡으며 해당 처리가 가능한지 체크부터 서버에 해당 처리에 대한 요청과 그에 대한 답변을 받습니다. 또한 그 답변을 토대로 데이터 처리를 완료 한 후 구독중인 옵저버 들에게 어떤 처리가 실행되었는지 전달합니다.
