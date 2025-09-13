# 개요

회사에 근무하며 UI 콘텐츠 개발할 때 UI에서 사용하던 구조에 대한 코드입니다.

주로 MVP 패턴과 옵저버 패턴을 활용하였습니다.

![UML 이미지](https://github.com/ladius3565/Portfolio/blob/main/%EB%94%94%EC%9E%90%EC%9D%B8%20%ED%8C%A8%ED%84%B4%20%ED%99%9C%EC%9A%A9/Image/DesignPattern_UML.png)

위 UML 이미지 처럼 구조를 잡아 사용하였습니다.

가장 처음에는 MVP 패턴이 아닌 MVC 패턴으로 구조를 잡으려 했지만 View가 Model에 대해 알고 있어야 하고 

또한 Model에서 입력 이벤트에 의해 정보 업데이트가 이루어져 UI에서 갱신이 되어야 할 때 전달해주는 방식은 옵저버 패턴을 활용하였습니다.

옵저버 패턴을 사용함으로 여러 UI에서 동시에 정보 업데이트에 대한 호출을 받을 수 있게 되는데 
