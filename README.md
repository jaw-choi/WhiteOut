# WhiteOut

Unity 6.3 URP 기반의 작은 쿼터뷰 생존 게임 프로젝트입니다. 플레이어는 눈보라 속에서 장작을 모으고, 화톳불을 유지하며, 도구와 음식을 자동 제작해 가능한 오래 생존합니다.

## 게임 목표

체온과 체력이 0이 되지 않게 유지하면서 생존 시간을 늘립니다. 장작은 화톳불 연료와 제작 재료로 쓰이고, 음식은 체력 회복 또는 돈 전환으로 처리됩니다. 최고 생존 시간은 `PlayerPrefs`에 저장됩니다.

## 핵심 시스템 요약

- 입력: New Input System, PC 키보드와 Android 가상 조이스틱 공용 이동
- 이동: `CharacterController` 기반 평지 이동, 점프 없음
- 카메라: 고정 각도 쿼터뷰 추적 카메라
- 생존: 체온 감소, 눈보라 강도 상승, 체온 0일 때 체력 감소
- 화톳불: 자동 점화, 자동 연료 보충, 열 범위 안 체온 회복
- 자원: 장작 수집, 도구 자동 제작, 음식 자동 생성, 체력/돈 자동 처리
- UI: HUD, 게임오버 패널, 재시작 버튼, 최고 기록 표시

## 폴더 구조

```text
Assets/_Project
Assets/_Project/Input
Assets/_Project/Scripts/Core
Assets/_Project/Scripts/Config
Assets/_Project/Scripts/Player
Assets/_Project/Scripts/Camera
Assets/_Project/Scripts/Inventory
Assets/_Project/Scripts/Systems
Assets/_Project/Scripts/World
Assets/_Project/Scripts/UI
Assets/_Project/Prefabs
Assets/_Project/Scenes
Assets/_Project/Art
Assets/_Project/Audio
Assets/_Project/UI
```

## 커밋 진행 순서

1. 프로젝트 구조, 입력 액션, 공용 상수
2. 플레이어 이동, 쿼터뷰 카메라, 모바일 조이스틱
3. 생존 스탯, 눈보라, 게임오버 상태
4. 장작 수집, 인벤토리, 화톳불 자동화
5. 도구 제작, 음식 생성, 돈 전환
6. HUD, 최고 기록 저장, 게임오버 UI
7. README와 Editor 수동 작업 문서화

## 코드가 자동 처리하는 일

- `WoodPickup`은 플레이어가 트리거에 들어오면 장작을 자동 획득합니다.
- `CampfireController`는 가까운 플레이어가 장작을 갖고 있으면 자동 점화 및 자동 연료 보충을 합니다.
- `CampfireHeatZone`은 플레이어가 열 범위에 들어오고 나가는 것을 `SurvivalSystem`에 전달합니다.
- `CraftingSystem`은 장작 5개로 도구를 자동 제작하고, 화톳불 근처에서 음식 제작과 체력/돈 처리를 자동 수행합니다.
- `GameFlowController`는 생존 시간, 게임오버, 재시작, 최고 기록 저장을 관리합니다.
- `HUDController`와 `GameOverPanelController`는 시스템 이벤트를 받아 UI를 갱신합니다.

## Editor 수동 작업 가이드

이 섹션은 빈 Unity 6.3 URP 프로젝트에서 씬을 직접 구성하는 절차입니다. 오브젝트 이름은 아래와 동일하게 만드는 것을 권장합니다.

### 1. 프로젝트 준비

1. Unity Hub에서 `New project`를 누릅니다.
2. 템플릿에서 `Universal 3D` 또는 `URP` 템플릿을 선택합니다.
3. 프로젝트 이름을 `WhiteOut`으로 만들고 엽니다.
4. 상단 메뉴에서 `Window > Package Manager`를 엽니다.
5. `Unity Registry`에서 `Input System`이 설치되어 있는지 확인합니다.
6. 설치되어 있지 않으면 `Input System`을 선택하고 `Install`을 누릅니다.
7. 상단 메뉴에서 `Edit > Project Settings > Player`를 엽니다.
8. `Other Settings > Active Input Handling`을 `Input System Package (New)` 또는 `Both`로 설정합니다.
9. Unity가 재시작을 요구하면 재시작합니다.

### 2. 메인 씬 생성

1. Project 창에서 `Assets/_Project/Scenes` 폴더를 엽니다.
2. 우클릭 후 `Create > Scene`을 선택합니다.
3. 씬 이름을 `Main`으로 변경합니다.
4. `Main.unity`를 더블클릭해 엽니다.
5. 상단 메뉴에서 `File > Save`를 누릅니다.
6. 상단 메뉴에서 `File > Build Profiles` 또는 `File > Build Settings`를 엽니다.
7. `Add Open Scenes`를 눌러 `Main` 씬을 빌드 목록에 추가합니다.

### 3. 밸런스 에셋 생성

1. Project 창에서 `Assets/_Project` 아래에 `Config` 폴더가 없다면 만듭니다.
2. `Assets/_Project/Config` 폴더에서 우클릭합니다.
3. `Create > WhiteOut > Game Balance Config`를 선택합니다.
4. 에셋 이름을 `GameBalanceConfig`로 변경합니다.
5. Inspector에서 기본값을 확인합니다.
6. 권장 기본값은 `Starting Health=100`, `Max Health=100`, `Starting Temperature=100`, `Max Temperature=100`, `Base Temperature Loss Per Second=1`, `Temperature Recovery Per Second=5`, `Health Loss When Frozen Per Second=2`, `Starting Campfire Burn Seconds=30`, `Burn Seconds Per Wood=3`, `Tool Wood Cost=5`, `Food Wood Cost=5`, `Food Heal Amount=1`, `Money Per Overflow Food=10`, `Auto Craft Check Interval=0.25`입니다.

### 4. Tag와 Layer 생성

1. 상단 메뉴에서 `Edit > Project Settings > Tags and Layers`를 엽니다.
2. `Tags` 목록에 `Player`, `Campfire`, `CampfireHeatZone`, `WoodPickup`을 추가합니다.
3. `Layers` 목록에 `Player`, `Campfire`, `CampfireHeatZone`, `WoodPickup`을 추가합니다.
4. 나중에 만든 오브젝트에 같은 이름의 Tag와 Layer를 지정합니다.
5. `CampfireController`의 `Player Search Mask`에는 `Player` 레이어를 선택하는 것을 권장합니다.

### 5. GameBootstrap 만들기

1. Hierarchy 창에서 우클릭합니다.
2. `Create Empty`를 선택합니다.
3. 이름을 `GameBootstrap`으로 변경합니다.
4. Inspector에서 `Add Component`를 누르고 `BlizzardSystem`을 추가합니다.
5. `Add Component`를 누르고 `SurvivalSystem`을 추가합니다.
6. `Add Component`를 누르고 `GameFlowController`를 추가합니다.
7. `Add Component`를 누르고 `CraftingSystem`을 추가합니다.
8. `SurvivalSystem > Balance Config`에 `GameBalanceConfig` 에셋을 드래그합니다.
9. `SurvivalSystem > Blizzard System`에 같은 오브젝트의 `BlizzardSystem` 컴포넌트를 드래그합니다.
10. `GameFlowController > Survival System`에 같은 오브젝트의 `SurvivalSystem` 컴포넌트를 드래그합니다.
11. `BlizzardSystem > Game Flow Controller`에 같은 오브젝트의 `GameFlowController` 컴포넌트를 드래그합니다.
12. `CraftingSystem > Balance Config`에 `GameBalanceConfig` 에셋을 드래그합니다.
13. `CraftingSystem > Survival System`에 `SurvivalSystem` 컴포넌트를 드래그합니다.
14. `CraftingSystem > Game Flow Controller`에 `GameFlowController` 컴포넌트를 드래그합니다.
15. `CraftingSystem > Player Inventory`는 Player를 만든 뒤 연결합니다.

### 6. Player 만들기

1. Hierarchy 창에서 우클릭합니다.
2. `3D Object > Capsule`을 선택합니다.
3. 이름을 `Player`로 변경합니다.
4. Inspector 상단에서 Tag를 `Player`로 설정합니다.
5. Layer를 `Player`로 설정합니다.
6. `Transform` 값을 `Position=(0, 1, 0)`, `Rotation=(0, 0, 0)`, `Scale=(1, 1, 1)`로 둡니다.
7. `Add Component`를 눌러 `CharacterController`를 추가합니다.
8. `CharacterController` 권장값은 `Center=(0, 1, 0)`, `Radius=0.45`, `Height=2`, `Slope Limit=45`, `Step Offset=0.3`입니다.
9. `Add Component`를 눌러 `PlayerInput`을 추가합니다.
10. `PlayerInput > Actions`에 `Assets/_Project/Input/BlizzardSurvival.inputactions`를 드래그합니다.
11. `PlayerInput > Default Map`을 `Gameplay`로 설정합니다.
12. `PlayerInput > Behavior`는 `Invoke Unity Events` 또는 `Send Messages` 어느 쪽도 가능하지만, 이 프로젝트 코드는 `PlayerInputReader`가 액션을 직접 읽으므로 필수 이벤트 연결은 없습니다.
13. `Add Component`를 눌러 `PlayerInputReader`를 추가합니다.
14. `PlayerInputReader > Player Input`에 같은 오브젝트의 `PlayerInput`을 드래그합니다.
15. `PlayerInputReader > Move Action Name`이 `Move`인지 확인합니다.
16. `Add Component`를 눌러 `PlayerMover`를 추가합니다.
17. `PlayerMover > Input Reader`에 같은 오브젝트의 `PlayerInputReader`를 드래그합니다.
18. `PlayerMover > Character Controller`에 같은 오브젝트의 `CharacterController`를 드래그합니다.
19. `PlayerMover > Game Flow Controller`에 `GameBootstrap`의 `GameFlowController`를 드래그합니다.
20. `PlayerMover > Move Speed`는 `4.5`, `Rotation Sharpness`는 `12`로 둡니다.
21. `Add Component`를 눌러 `PlayerInventory`를 추가합니다.
22. `GameBootstrap`을 선택하고 `CraftingSystem > Player Inventory`에 `Player`의 `PlayerInventory`를 드래그합니다.

### 7. 카메라 설정

1. Hierarchy에서 기존 `Main Camera`를 선택하거나 새 카메라를 만듭니다.
2. 이름을 `MainCamera`로 변경합니다.
3. Tag를 `MainCamera`로 둡니다.
4. `Transform`은 임시로 `Position=(0, 10, -8)`, `Rotation=(50, 0, 0)`로 둡니다.
5. `Add Component`를 눌러 `QuarterViewCameraFollow`를 추가합니다.
6. `QuarterViewCameraFollow > Target`에 `Player`를 드래그합니다.
7. `Offset`은 `(0, 10, -8)`을 권장합니다.
8. `Follow Smooth`는 `10`, `Look At Target Height`는 `1.5`를 권장합니다.
9. `Player`를 선택하고 `PlayerMover > Movement Reference`에 `MainCamera` Transform을 드래그합니다.

### 8. 바닥과 맵 오브젝트 배치

1. Hierarchy에서 우클릭합니다.
2. `3D Object > Plane`을 선택합니다.
3. 이름을 `Ground`로 변경합니다.
4. `Transform > Scale`을 `(5, 1, 5)` 정도로 설정합니다.
5. 무료 에셋을 쓴다면 `Assets/_Project/Art` 아래에 임포트하고, 나무/돌/눈 오브젝트를 수동 배치합니다.
6. 이동은 평지 전용이므로 테스트 단계에서는 바닥 높이를 일정하게 유지합니다.

### 9. 화톳불 3개 만들기

각 화톳불은 같은 구조로 만듭니다. 이름만 `Campfire_Main`, `Campfire_North`, `Campfire_East`로 다르게 합니다.

1. Hierarchy에서 우클릭합니다.
2. `Create Empty`를 선택합니다.
3. 첫 번째 이름을 `Campfire_Main`으로 변경합니다.
4. Tag를 `Campfire`, Layer를 `Campfire`로 설정합니다.
5. `Transform > Position`을 예를 들어 `(0, 0, 3)`으로 설정합니다.
6. `Add Component`를 눌러 `CampfireController`를 추가합니다.
7. `CampfireController > Balance Config`에 `GameBalanceConfig` 에셋을 드래그합니다.
8. `CampfireController > Start Lit`은 `Campfire_Main`만 체크합니다.
9. `CampfireController > Auto Ignite Radius`는 `2`, `Heat Radius`는 `3`을 권장합니다.
10. `CampfireController > Auto Fuel Check Interval`은 `0.25`, `Auto Refuel Below Seconds`는 `1`을 권장합니다.
11. `CampfireController > Player Search Mask`에서 `Player` 레이어를 선택합니다.
12. `Campfire_Main` 아래에 우클릭 후 `3D Object > Cylinder`를 만들어 시각 오브젝트로 씁니다.
13. 자식 Cylinder 이름을 `CampfireVisual`로 변경합니다.
14. `CampfireVisual`의 Scale을 `(1, 0.2, 1)` 정도로 줄입니다.
15. `Campfire_Main` 아래에 우클릭 후 `Create Empty`를 선택합니다.
16. 자식 이름을 `HeatZone`으로 변경합니다.
17. `HeatZone`의 Tag를 `CampfireHeatZone`, Layer를 `CampfireHeatZone`으로 설정합니다.
18. `HeatZone`에 `SphereCollider`를 추가합니다.
19. `SphereCollider > Is Trigger`를 체크합니다.
20. `SphereCollider > Radius`를 `3`으로 설정합니다.
21. `HeatZone`에 `CampfireHeatZone` 컴포넌트를 추가합니다.
22. `CampfireHeatZone > Survival System`에 `GameBootstrap`의 `SurvivalSystem`을 드래그합니다.
23. `CampfireHeatZone > Campfire Controller`에 부모의 `CampfireController`를 드래그합니다.
24. `CampfireHeatZone > Trigger Collider`에 같은 오브젝트의 `SphereCollider`를 드래그합니다.
25. `CampfireHeatZone > Require Lit Campfire`를 체크합니다.
26. `Campfire_North`는 Position을 예를 들어 `(-4, 0, 8)`로 설정하고 `Start Lit`은 끕니다.
27. `Campfire_East`는 Position을 예를 들어 `(5, 0, 2)`로 설정하고 `Start Lit`은 끕니다.
28. 세 화톳불 모두 같은 방식으로 `HeatZone` 자식을 구성합니다.

### 10. 장작 픽업 프리팹 만들기

1. Hierarchy에서 우클릭합니다.
2. `3D Object > Cube`를 선택합니다.
3. 이름을 `WoodPickup`으로 변경합니다.
4. Tag를 `WoodPickup`, Layer를 `WoodPickup`으로 설정합니다.
5. `Transform > Scale`을 `(0.4, 0.2, 0.8)` 정도로 설정합니다.
6. `BoxCollider`의 `Is Trigger`를 체크합니다.
7. `Add Component`를 눌러 `WoodPickup` 스크립트를 추가합니다.
8. `WoodPickup > Wood Amount`는 `1`로 둡니다.
9. `Destroy On Collect`는 꺼두면 획득 시 비활성화됩니다.
10. Project 창에서 `Assets/_Project/Prefabs` 폴더를 엽니다.
11. Hierarchy의 `WoodPickup`을 `Assets/_Project/Prefabs`로 드래그해 프리팹으로 저장합니다.
12. 씬에 여러 개 배치하려면 프리팹을 Ground 위 원하는 위치에 드래그합니다.
13. 최소 15개 이상 배치하면 도구 제작과 음식 루프 테스트가 쉽습니다.

### 11. Canvas와 HUD 만들기

1. Hierarchy에서 우클릭합니다.
2. `UI > Canvas`를 선택합니다.
3. 이름을 `Canvas`로 변경합니다.
4. `Canvas > Render Mode`는 `Screen Space - Overlay`로 둡니다.
5. Canvas 아래에 우클릭 후 `UI > Panel`을 선택합니다.
6. 패널 이름을 `HUD`로 변경합니다.
7. `HUD`의 RectTransform을 화면 왼쪽 위에 배치합니다.
8. `HUD`에 `HUDController` 컴포넌트를 추가합니다.
9. `HUDController > Survival System`에 `GameBootstrap`의 `SurvivalSystem`을 드래그합니다.
10. `HUDController > Player Inventory`에 `Player`의 `PlayerInventory`를 드래그합니다.
11. `HUDController > Game Flow Controller`에 `GameBootstrap`의 `GameFlowController`를 드래그합니다.
12. `HUD` 아래에 우클릭 후 `UI > Slider`를 선택합니다.
13. 이름을 `TemperatureBar`로 변경합니다.
14. `TemperatureBar`를 복제하거나 새 Slider를 만들고 이름을 `HealthBar`로 변경합니다.
15. `HUDController > Temperature Bar`에 `TemperatureBar`를 드래그합니다.
16. `HUDController > Health Bar`에 `HealthBar`를 드래그합니다.
17. `HUD` 아래에 텍스트를 6개 만듭니다.
18. TextMeshPro를 쓴다면 `UI > Text - TextMeshPro`, 기본 Text를 쓴다면 `UI > Legacy > Text`를 사용합니다.
19. 텍스트 오브젝트 이름을 `WoodCountText`, `ToolCountText`, `FoodCountText`, `MoneyCountText`, `SurvivalTimeText`, `BestTimeText`로 변경합니다.
20. `HUDController`의 각 `UILabel` 필드에서 `Text` 또는 `Tmp Text` 슬롯에 해당 텍스트 컴포넌트를 드래그합니다.
21. 예를 들어 `Wood Count Text > Tmp Text`에 `WoodCountText`의 TMP_Text를 드래그합니다.
22. 모든 텍스트가 화면 안에 보이도록 RectTransform 위치를 조정합니다.

### 12. 모바일 가상 조이스틱 만들기

1. `Canvas` 아래에 우클릭 후 `Create Empty`를 선택합니다.
2. 이름을 `MobileControls`로 변경합니다.
3. `MobileControls` 아래에 우클릭 후 `UI > Panel` 또는 `UI > Image`를 만듭니다.
4. 이름을 `VirtualJoystickRoot`로 변경합니다.
5. `VirtualJoystickRoot`의 RectTransform을 화면 왼쪽 아래에 배치합니다.
6. `VirtualJoystickRoot`에 `Image`가 없다면 추가하고 반투명 원형 배경처럼 설정합니다.
7. `VirtualJoystickRoot` 이름을 유지한 상태로 `VirtualJoystick` 컴포넌트를 추가합니다.
8. `VirtualJoystickRoot` 아래에 `UI > Image`를 하나 더 만듭니다.
9. 자식 이름을 `JoystickHandle`로 변경합니다.
10. `JoystickHandle`은 배경보다 작게 만들고 중앙에 둡니다.
11. `VirtualJoystick > Background`에 `VirtualJoystickRoot` RectTransform을 드래그합니다.
12. `VirtualJoystick > Handle`에 `JoystickHandle` RectTransform을 드래그합니다.
13. `VirtualJoystick > Game Flow Controller`에 `GameBootstrap`의 `GameFlowController`를 드래그합니다.
14. `VirtualJoystick > Visual Root`에 `MobileControls` 또는 `VirtualJoystickRoot`를 드래그합니다.
15. `Player`를 선택합니다.
16. `PlayerInputReader > Virtual Joystick`에 `VirtualJoystickRoot`의 `VirtualJoystick` 컴포넌트를 드래그합니다.
17. PC 테스트에서 조이스틱이 방해되면 `MobileControls`를 비활성화해도 됩니다.

### 13. 게임오버 패널 만들기

1. `Canvas` 아래에 우클릭 후 `UI > Panel`을 선택합니다.
2. 이름을 `GameOverPanel`로 변경합니다.
3. 화면 전체를 덮도록 RectTransform을 stretch로 설정합니다.
4. Panel 색상은 어두운 반투명 색을 권장합니다.
5. `GameOverPanel` 아래에 `UI > Text - TextMeshPro` 또는 `UI > Legacy > Text`를 만듭니다.
6. 이름을 `GameOverText`로 변경합니다.
7. 가운데 정렬하고 글자 크기를 크게 설정합니다.
8. `GameOverPanel` 아래에 `UI > Button` 또는 `UI > Button - TextMeshPro`를 만듭니다.
9. 이름을 `RestartButton`으로 변경합니다.
10. 버튼 텍스트를 `Restart`로 변경합니다.
11. `GameOverPanel` 또는 `Canvas`에 `GameOverPanelController` 컴포넌트를 추가합니다.
12. `GameOverPanelController > Game Flow Controller`에 `GameBootstrap`의 `GameFlowController`를 드래그합니다.
13. `GameOverPanelController > Game Over Panel`에 `GameOverPanel` 오브젝트를 드래그합니다.
14. `GameOverPanelController > Game Over Text` 또는 `Game Over Tmp Text`에 `GameOverText` 컴포넌트를 드래그합니다.
15. `GameOverPanelController > Restart Button`에 `RestartButton`의 Button 컴포넌트를 드래그합니다.
16. `RestartButton`의 OnClick은 코드가 자동으로 등록하므로 수동으로 추가하지 않아도 됩니다.
17. 시작 시 패널을 숨기고 싶으면 `GameOverPanel`을 비활성화해도 됩니다. `GameOverPanelController`가 게임오버 이벤트를 받으면 다시 켭니다.

### 14. Input Actions 확인

1. Project 창에서 `Assets/_Project/Input/BlizzardSurvival.inputactions`를 선택합니다.
2. Inspector에서 `Edit asset` 또는 Input Actions 편집기를 엽니다.
3. `Gameplay` 액션맵이 있는지 확인합니다.
4. `Gameplay/Move`가 `Vector2`인지 확인합니다.
5. `Gameplay/Restart`가 `Button`인지 확인합니다.
6. `Move`에 WASD와 Arrow 키 바인딩이 있는지 확인합니다.
7. `Player`의 `PlayerInput > Actions`가 이 에셋을 참조하는지 다시 확인합니다.

### 15. 프리팹 저장 권장 위치

```text
Assets/_Project/Prefabs/Player.prefab
Assets/_Project/Prefabs/WoodPickup.prefab
Assets/_Project/Prefabs/Campfire.prefab
Assets/_Project/Prefabs/HUD.prefab
Assets/_Project/Prefabs/MobileControls.prefab
Assets/_Project/Prefabs/GameOverPanel.prefab
```

프리팹 저장은 필수는 아니지만, 장작과 화톳불을 반복 배치할 때 실수를 줄입니다.

### 16. 플레이 테스트 절차

1. 상단의 Play 버튼을 누릅니다.
2. WASD와 방향키로 `Player`가 움직이는지 확인합니다.
3. `MainCamera`가 플레이어를 따라가는지 확인합니다.
4. 시간이 지나며 `TemperatureBar`가 감소하는지 확인합니다.
5. `GameBootstrap > BlizzardSystem > Current Severity` 값이 시간이 지나며 증가하는지 Inspector Debug 모드에서 확인합니다.
6. 장작에 닿으면 `WoodCountText`가 증가하는지 확인합니다.
7. 장작 5개를 모으면 `ToolCountText`가 `Tool: Owned`로 바뀌는지 확인합니다.
8. 꺼진 화톳불 근처에 가면 장작 1개가 소모되고 화톳불이 켜지는지 확인합니다.
9. 켜진 화톳불 열 범위 안에서 `TemperatureBar`가 회복되는지 확인합니다.
10. 도구 보유 후 화톳불 근처에서 장작 5개가 있으면 음식 생성 후 체력 회복 또는 돈 전환이 되는지 확인합니다.
11. `GameBalanceConfig`에서 `Base Temperature Loss Per Second`와 `Health Loss When Frozen Per Second`를 크게 올려 빠르게 게임오버를 확인합니다.
12. 게임오버 시 `GameOverPanel`이 나타나는지 확인합니다.
13. `RestartButton`을 눌렀을 때 현재 씬이 다시 로드되는지 확인합니다.
14. 다시 게임오버를 만들고 `BestTimeText`가 이전 최고 기록을 유지하는지 확인합니다.

### 17. Android 빌드 세팅

1. 상단 메뉴에서 `File > Build Profiles` 또는 `File > Build Settings`를 엽니다.
2. Platform 목록에서 `Android`를 선택합니다.
3. `Switch Platform`을 누릅니다.
4. `Main` 씬이 `Scenes In Build` 목록에 있는지 확인합니다.
5. 상단 메뉴에서 `Edit > Project Settings > Player`를 엽니다.
6. Android 탭을 선택합니다.
7. `Company Name`과 `Product Name`을 설정합니다.
8. `Other Settings > Package Name`을 예: `com.yourname.whiteout`처럼 고유하게 설정합니다.
9. `Resolution and Presentation > Default Orientation`은 `Landscape Left` 또는 `Portrait` 중 원하는 방향으로 정합니다.
10. 모바일 조이스틱 위치는 선택한 방향에서 손가락이 닿기 쉬운 곳으로 조정합니다.
11. `Other Settings > Active Input Handling`이 `Input System Package (New)` 또는 `Both`인지 확인합니다.
12. Android SDK/NDK 경고가 뜨면 Unity Hub에서 Android Build Support, Android SDK & NDK Tools, OpenJDK를 설치합니다.
13. 기기 테스트 전 `MobileControls`가 활성화되어 있는지 확인합니다.
14. Editor에서 터치 시뮬레이션이 필요하면 Input Debugger 또는 Game 뷰의 터치 시뮬레이션 옵션을 사용합니다.

### 18. 자주 나는 실수

- `PlayerInput.actions`에 `BlizzardSurvival.inputactions`를 연결하지 않으면 키보드 이동이 안 됩니다.
- `PlayerInputReader.virtualJoystick` 연결이 빠지면 모바일 조이스틱 입력이 이동에 합쳐지지 않습니다.
- `WoodPickup`의 Collider에서 `Is Trigger`가 꺼져 있으면 장작 획득이 안 됩니다.
- `HeatZone`의 `SphereCollider.Is Trigger`가 꺼져 있으면 체온 회복 진입/이탈이 안 됩니다.
- `CampfireController.Player Search Mask`에 `Player` 레이어가 빠지면 자동 점화가 안 됩니다.
- `Campfire_Main.Start Lit`을 켜지 않으면 시작 화톳불도 꺼진 상태로 시작합니다.
- `GameBalanceConfig`를 `SurvivalSystem`, `CraftingSystem`, `CampfireController`에 연결하지 않으면 기본 fallback 값으로 동작합니다.
- `GameOverPanelController.Restart Button` 연결이 빠지면 재시작 버튼이 동작하지 않습니다.
- `Canvas`에 `EventSystem`이 없으면 UI 버튼과 조이스틱 드래그가 동작하지 않을 수 있습니다. Unity가 자동 생성하지 않았다면 `UI > Event System`을 만듭니다.
- 최고 기록은 `PlayerPrefs`에 저장되므로 Editor에서 값을 초기화하려면 `HighScoreService.ClearBestSurvivalTime()`을 임시 디버그 코드로 호출하거나 PlayerPrefs를 삭제해야 합니다.

### 19. 연결 누락 체크리스트

- `GameBootstrap`: `BlizzardSystem`, `SurvivalSystem`, `GameFlowController`, `CraftingSystem` 추가 완료
- `SurvivalSystem`: `Balance Config`, `Blizzard System` 연결 완료
- `GameFlowController`: `Survival System` 연결 완료
- `CraftingSystem`: `Balance Config`, `Player Inventory`, `Survival System`, `Game Flow Controller` 연결 완료
- `Player`: `CharacterController`, `PlayerInput`, `PlayerInputReader`, `PlayerMover`, `PlayerInventory` 추가 완료
- `PlayerInput`: `BlizzardSurvival.inputactions`, `Default Map=Gameplay` 설정 완료
- `PlayerMover`: `Input Reader`, `Character Controller`, `Game Flow Controller`, 필요 시 `Movement Reference=MainCamera` 연결 완료
- `MainCamera`: `QuarterViewCameraFollow.Target=Player` 연결 완료
- 각 `Campfire`: `CampfireController`, `Balance Config`, `Player Search Mask=Player` 설정 완료
- 각 `HeatZone`: `SphereCollider.Is Trigger`, `CampfireHeatZone`, `Survival System`, `Campfire Controller` 연결 완료
- 각 `WoodPickup`: Collider `Is Trigger`, `WoodPickup` 컴포넌트 추가 완료
- `HUD`: `HUDController`, Slider 2개, 텍스트 6개 연결 완료
- `VirtualJoystickRoot`: `VirtualJoystick`, `Background`, `Handle`, `Game Flow Controller`, `Visual Root` 연결 완료
- `GameOverPanel`: `GameOverPanelController`, `Game Over Panel`, `Game Over Text`, `Restart Button` 연결 완료
- Build Settings: `Assets/_Project/Scenes/Main.unity` 추가 완료

## 추천 커밋 메시지

```text
docs: add detailed setup and editor workflow guide
```
