# 개요
[돌아가기](https://github.com/ladius3565/Portfolio/blob/main/README.md)


해당 코드는 스크롤이 포함된 UI에서 최적화를 위해 무한 스크롤을 구현한 코드입니다.

유니티의 ScrollRect 컴포넌트와 함께 사용하도록 제작되었습니다.

<p align="center">
  <img src="https://github.com/ladius3565/Portfolio/blob/main/%EA%B3%B5%ED%86%B5%20%EA%B8%B0%EB%8A%A5%20%EA%B5%AC%ED%98%84/%EB%AC%B4%ED%95%9C%20%EC%8A%A4%ED%81%AC%EB%A1%A4/Image/ScrollUML.png">
</p>

스크롤에 사용되는 데이터의 경우에는 데이터 수정이 자유롭게 가능하도록 List를 사용하였으며 슬롯의 경우에는 스크롤시 특정 위치의 슬롯에만 접근하기 보다는 순차적으로 처리하기 때문에 LinkedList를 사용하였습니다.

해당 무한 스크롤의 기본적인 원리는 스크롤 범위에 필요한 수 만큼의 슬롯을 생성하여 보유한 뒤 스크롤 방향에 따라 슬롯과 스크롤 범위를 계산하는데

<p align="center">
  <img width="48%" src="https://github.com/ladius3565/Portfolio/blob/main/%EA%B3%B5%ED%86%B5%20%EA%B8%B0%EB%8A%A5%20%EA%B5%AC%ED%98%84/%EB%AC%B4%ED%95%9C%20%EC%8A%A4%ED%81%AC%EB%A1%A4/Image/Scroll1.png">
</p>

사진과 같이 만약 위로 스크롤 하고 있는 상황이라면 가장 상단에 위치한 슬롯의 하단 Y 값과 스크롤 범위의 Y을 계산하여 만약 해당 슬롯이 스크롤 범위를 벗어났다 판단되면 최하단으로 슬롯의 위치를 변경해주고 다음으로 상단에 위치하는 슬롯을 검사 하여 더이상 스크롤 범위를 벗어난 슬롯이 없다 판단되면 멈추게 됩니다.

반대의 상황의 경우에는 가장 하단에 위치한 슬롯의 상단 Y 값과 (스크롤 범위 y값 - 전체 스크롤 높이)을 비교하여 스크롤 범위를 벗어나면 최상단으로 위치를 변경해줍니다.

<p align="center" >
  <img src="https://github.com/ladius3565/Portfolio/blob/main/%EA%B3%B5%ED%86%B5%20%EA%B8%B0%EB%8A%A5%20%EA%B5%AC%ED%98%84/%EB%AC%B4%ED%95%9C%20%EC%8A%A4%ED%81%AC%EB%A1%A4/Image/%EB%AC%B4%ED%95%9C%EC%8A%A4%ED%81%AC%EB%A1%A4.gif">
</p>

실제로 유니티에서 적용된 모습은 위와 같이 스크롤 방향에 따라 반대편에 위치한 슬롯의 위치와 스크롤 범위를 계산하여 옮겨주는 모습입니다.

성능 비교를 위해 0 ~ 5000 까지의 데이터를 가진 스크롤을 세팅하였습니다. (텍스트만 보유한 스크롤이라 확실한 비교를 위해 5000으로 세팅)

![](https://github.com/ladius3565/Portfolio/blob/main/%EA%B3%B5%ED%86%B5%20%EA%B8%B0%EB%8A%A5%20%EA%B5%AC%ED%98%84/%EB%AC%B4%ED%95%9C%20%EC%8A%A4%ED%81%AC%EB%A1%A4/Image/WorstScroll.png) 기본 스크롤 | ![](https://github.com/ladius3565/Portfolio/blob/main/%EA%B3%B5%ED%86%B5%20%EA%B8%B0%EB%8A%A5%20%EA%B5%AC%ED%98%84/%EB%AC%B4%ED%95%9C%20%EC%8A%A4%ED%81%AC%EB%A1%A4/Image/InfiniteScroll.png) 무한 스크롤
---|---|

이미지와 같이 움직이지 않고 두더라도 프레임 차이가 발생하며 스크롤을 움직이면 프레임이 더 떨어지게 됩니다.

또한 실제로 사용시에 이미지와 여러개의 텍스트가 추가 될 경우 더 적은 데이터를 보유하더라도 프레임 드랍이 발생하게 되므로 무한 스크롤이 필요하게 됩니다.

![](https://github.com/ladius3565/Portfolio/blob/main/%EA%B3%B5%ED%86%B5%20%EA%B8%B0%EB%8A%A5%20%EA%B5%AC%ED%98%84/%EB%AC%B4%ED%95%9C%20%EC%8A%A4%ED%81%AC%EB%A1%A4/Image/HorizontalScroll.png) 수평 스크롤 | ![](https://github.com/ladius3565/Portfolio/blob/main/%EA%B3%B5%ED%86%B5%20%EA%B8%B0%EB%8A%A5%20%EA%B5%AC%ED%98%84/%EB%AC%B4%ED%95%9C%20%EC%8A%A4%ED%81%AC%EB%A1%A4/Image/GridScroll.png) 그리드 스크롤 
---|---|

위의 코드를 활용하면 수평 또는 그리드 형태의 스크롤 또한 만들 수 있습니다.
