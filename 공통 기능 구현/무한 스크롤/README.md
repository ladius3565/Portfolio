# 개요
[돌아가기](https://github.com/ladius3565/Portfolio/blob/main/README.md)


해당 코드는 스크롤이 포함된 UI에서 최적화를 위해 무한 스크롤을 구현한 코드입니다.

(코드 설명)


--------------------------

성능 비교를 위해 0 ~ 5000 까지의 데이터를 가진 스크롤을 세팅하였습니다. (텍스트만 보유한 스크롤이라 확실한 비교를 위해 5000으로 세팅)

![](https://github.com/ladius3565/Portfolio/blob/main/%EA%B3%B5%ED%86%B5%20%EA%B8%B0%EB%8A%A5%20%EA%B5%AC%ED%98%84/%EB%AC%B4%ED%95%9C%20%EC%8A%A4%ED%81%AC%EB%A1%A4/Image/WorstScroll.png) 기본 스크롤 | ![](https://github.com/ladius3565/Portfolio/blob/main/%EA%B3%B5%ED%86%B5%20%EA%B8%B0%EB%8A%A5%20%EA%B5%AC%ED%98%84/%EB%AC%B4%ED%95%9C%20%EC%8A%A4%ED%81%AC%EB%A1%A4/Image/InfiniteScroll.png) 무한 스크롤
---|---|

이미지와 같이 움직이지 않고 두더라도 프레임 차이가 발생하며 스크롤을 움직이면 프레임이 더 떨어지게 됩니다.

또한 실제로 사용시에 이미지와 여러개의 텍스트가 추가 될 경우 더 적은 데이터를 보유하더라도 프레임 드랍이 발생하게 되므로 무한 스크롤이 필요하게 됩니다.

(수평 스크롤 이미지) | (그리드 스크롤 이미지)

위의 코드를 활용하면 수평, 그리드 형태의 스크롤 또한 만들 수 있습니다.
