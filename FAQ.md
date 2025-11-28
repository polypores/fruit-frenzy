01. [XONG] Nếu muốn lưu volume hiện tại thì làm sao?
Trong code.
02. [XONG] Làm cho nhạc dừng khi ra Menu chính?
Trong code.
03. [XONG] Thêm nhạc nền vào Main Menu?
Trong code.
04. [XONG] Làm cho tiếng SFX lớn hơn tiếng nhạc?
Enable/disable trong Hiearachy. Đã code sẵn SFX. Kéo lại SFX cho đều giao diện.
05. [XONG] Thay đổi vị trí nút X của bảng chọn Credit qua trái?
Kéo thả trên giao diện Unity. Nhớ enable nút X.
06. [XONG] Quả cầu gai thường hay bay ra ngoài, cho quả cầu gai bay hướng vào trong hoặc hướng về người chơi.
Trong code.
07. [XONG] Làm cho quả cầu gai rơi thẳng xuống?
Thay đổi giá trị vx thành vx = 0, cho m.Launch(vx = 0), nhấn Ctrl + Shift + F để tìm nhanh biến.
08. [XONG] Làm cho 2 - 3 cầu gai rơi 1 lượt?
Đổi hàm yield return... trong code.
09. [XONG] Cho người chơi nhảy cao/thấp hơn 1 chút?
Cách 1: Thay đổi jumpForce trong PlayerControllerScript.cs
Cách 2: Chỉnh RigidBody 2D -> Mass của Player.
Ko chỉnh Gravity, chỉnh Gravity làm Player bay bổng khó chạm đất nhanh.
10. [XONG] Cho người chơi chạy nhanh/chậm hơn 1 chút?
Tăng moveSpeed trong PlayerControllerScript.cs
11. [XONG] Cho game hiển thị Window thay vì Fullscreen?
Trong code.
12. [XONG] Cho kích cỡ các nút trên giao diện to ra hơn 1 chút?
Vào tăng Scale X, Y lên 1.5 – 1.75, k tăng chỉnh kích thước bằng tay.
12. [XONG] Cho khoảng cách các nút trên giao diện to ra hơn 1 chút?
Đối với Main Menu, vào MainMenuScene -> Canvas (SavedSystem) -> MainMenuSystem -> ButtonGroup. Chỉnh Spacing tại Vertical Layout Group.
13. [XONG] Thêm nút Quit To Desktop ở Pause Menu?
Duplicate nút Exit ở MainMenu bưng vô PausedMenu, chỉnh lại giao diện. Đổi hàm OnExitClicked() sang phía ScoreManager.
14. [XONG] Thêm 1 loại trái cây (shit chẳng hạn) và cho rơi?
Copy Cherry, Copy CherrySpawner và đổi lại tên biến.
15. [XONG] Làm trái cây vừa rơi vừa xoay?
Trong code.
16. [XONG] Mỗi khi ăn mạng thì sẽ có 1 trái tim nhỏ từ ng chơi bắn lên thanh Heart?
Trong code.
17. [XONG] Thêm tiếng ting ting khi chạm 1 loại trái cây khác?
Trong code.
18. [XONG] Thay đổi điểm ng chơi được cộng?
Chỉnh giá trị hàm trong code.
19. [XONG] Tạo hiệu ứng +1, +2, +3 điểm tại vị trí ng chơi đứng?
Trong code. ScoreCanvas nhớ để Screen Space – Overlay.
20. [XONG] Thay icon cảnh báo khi có cầu gai?
Nhớ đừng disable prefab DangerNotification và kéo DangerNotification vào Warn Icon Prefab ở Missile Spawner.