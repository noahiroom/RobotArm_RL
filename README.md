# RobotAgent (Robot Arm RL)
- RobotArm box pointer

## Part1
- download Robot Arm
- 로봇 팔의 각 부분에 mesh collider, rigid body, configurable joint 의 컴포넌트 추가후 설정
  - convex, 중력, 움직일 수 있는 가동범위 설정
- ball 생성.
## Part2
- 박스위치 랜덤설정
## Part3
- 로봇팔 scripts ML Agents 만들기
  -  ReacherArmAgent.cs : 
    - Initialize() : 각오브젝트의 rigid body 받기, Goal 정보 초기화
    - OnEpisodeBegin() : 에피소드가 시작할때 오브젝트 위치 초기화, Goal 정보 초기화, Goal 위치계산 업데이트
    - CollectObservations() : 관찰하는 총 sensor 수 84 space size
    - OnActionReceived : vectorAction - 각 joint 3개 의 움직임마다 업데이트, Goal 위치계산 업데이트
  -  ReacherGoal.cs : Trigger가 Target과 충돌했거나, 머무르거나, 나갔을때의 이벤트 설정
## Part4
- 실행하기
  - config파일 설정(yaml) : 파라미터 입력
    - yaml 파일은 그 해당 이름이 Unity의 Robot의 Behavior Parameters의 Behavior Name과 같아야한다.
  - mlagents-learn config/trainer_config.yaml --run-id=RobotArmRay3_00(아나콘다 프롬프트)

## Part5
- Raycast Sensor 이용
  - hand부위에 Raycast Sensor 3D 붙이고
  - Target Tag 설정하기( Target Tag : Target box에 달아줌)

## Results
<img src="/RobotArm.JPG"  width="450" height="280">
