# 개요
[돌아가기](https://github.com/ladius3565/Portfolio/blob/main/README.md)

AI의 구현에 비헤이비어 트리를 활용하여 구현한 코드입니다.

### 결과물
<p align="center">
  <img width="45%" src="https://github.com/ladius3565/Portfolio/blob/main/%EC%9D%B8%EA%B2%8C%EC%9E%84%20%EC%BD%94%EB%93%9C/AI%20%ED%96%89%EB%8F%99%20(%EB%B9%84%ED%97%A4%EC%9D%B4%EB%B9%84%EC%96%B4%20%ED%8A%B8%EB%A6%AC)/Image/AI_Tree.png">
  <img width="45%" src="https://github.com/ladius3565/Portfolio/blob/main/%EC%9D%B8%EA%B2%8C%EC%9E%84%20%EC%BD%94%EB%93%9C/AI%20%ED%96%89%EB%8F%99%20(%EB%B9%84%ED%97%A4%EC%9D%B4%EB%B9%84%EC%96%B4%20%ED%8A%B8%EB%A6%AC)/Image/AI_BehaviorTree.gif">  
</p>
왼쪽 이미지의 구조대로 비헤이비어 트리를 구성하여 오른쪽 gif로 보이는 것처럼 AI 행동 패턴을 구현하였습니다. 기본적으로는 정해진 순찰 루트대로 이동을 하며 만약 시야 내에 적(플레이어)이 등장한다면 해당 위치로 이동하여 공격하고 다시 시야에 놓치게 되면 다시 순찰루트로 돌아가게 됩니다.

### 기본 구조
<p align="center">
  <img width="40%" src="https://github.com/ladius3565/Portfolio/blob/main/%EC%9D%B8%EA%B2%8C%EC%9E%84%20%EC%BD%94%EB%93%9C/AI%20%ED%96%89%EB%8F%99%20(%EB%B9%84%ED%97%A4%EC%9D%B4%EB%B9%84%EC%96%B4%20%ED%8A%B8%EB%A6%AC)/Image/Node_Interface.png">
  <img width="40%" src="https://github.com/ladius3565/Portfolio/blob/main/%EC%9D%B8%EA%B2%8C%EC%9E%84%20%EC%BD%94%EB%93%9C/AI%20%ED%96%89%EB%8F%99%20(%EB%B9%84%ED%97%A4%EC%9D%B4%EB%B9%84%EC%96%B4%20%ED%8A%B8%EB%A6%AC)/Image/Node_Abstract.png">
</p>

왼쪽이미지는 가장 기본이되는 interface와 enum으로 eNodeState는 해당 노드의 실행 결과에 따라 Success, Failure, Running을 리턴하며 INode에서 Excute는 실제로 해당 노드를 실행할 때 호출하며 자식노드의 호출이 모두 완료된 후에 초기화 처리를 위해 Reset 함수를 호출합니다.

오른쪽 이미지는 INode를 상속받는 노드 클래스로 추상 클래스로 작성하였습니다. LeafNode는 가장 자식으로 배치되며 실제로 어떤 작업을 수행하는 노드들이 상속받게 되고 CompositeNode는 자식 노드들을 보유하며 하나씩 순회하는 노드들이 상속받게 됩니다.

<p align="center">
<img width="40%" src="https://github.com/ladius3565/Portfolio/blob/main/%EC%9D%B8%EA%B2%8C%EC%9E%84%20%EC%BD%94%EB%93%9C/AI%20%ED%96%89%EB%8F%99%20(%EB%B9%84%ED%97%A4%EC%9D%B4%EB%B9%84%EC%96%B4%20%ED%8A%B8%EB%A6%AC)/Image/Node_Leaf.png">
<img width="40%" src="https://github.com/ladius3565/Portfolio/blob/main/%EC%9D%B8%EA%B2%8C%EC%9E%84%20%EC%BD%94%EB%93%9C/AI%20%ED%96%89%EB%8F%99%20(%EB%B9%84%ED%97%A4%EC%9D%B4%EB%B9%84%EC%96%B4%20%ED%8A%B8%EB%A6%AC)/Image/Node_Composite.png">
</p>

이후 실제로 수행을 담당하는 ActionNode와 ConditionNode는 LeaftNode를 상속받으며 생성시에 각각 eNodeState, bool 값을 리턴하는 함수를 등록하여 실행 시 해당 함수를 호출합니다.

자식 노드의 순회를 담당하는 SelectorNode, SequenceNode, ParalleNode는 CompositeNode를 상속받으며 등록된 자식 노드들을 각각의 규칙에 맞게 순회하여 실행하고 그 결과를 부모 노드에 리턴합니다.

### 구현 부
<p align="center">
  <img width="40%" src="https://github.com/ladius3565/Portfolio/blob/main/%EC%9D%B8%EA%B2%8C%EC%9E%84%20%EC%BD%94%EB%93%9C/AI%20%ED%96%89%EB%8F%99%20(%EB%B9%84%ED%97%A4%EC%9D%B4%EB%B9%84%EC%96%B4%20%ED%8A%B8%EB%A6%AC)/Image/Tree_Code.png">
</p>

코드에서 생성시 해당 방식으로 코드를 생성하여 노드 등록을 할 수 있습니다.
