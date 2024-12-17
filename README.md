# Ward of Fear / Project Hollow

Made by:
2602112833 - Jason Alverino
2602141783 - Priska Dellan Lanosta
2602112253 - Ferdinand Darmawan 
2602107820 - Andrew Avellino Oinfre 

Brief Game Story:
Ward of Fear diawali dengan seorang prajurit AS yang terjebak di dunia lain ketika misi infiltrasinya di Indonesia tidak sesuai rencana. Ia sekarang dihantui oleh mahkluk-mahkluk berbahaya di suatu rumah sakit dan harus menggunakan semua ke senjata yang ia miliki untuk bertahan selama mungkin.
 
Brief Game Explanation:
Ward of Fear adalah game survival horror rogue-like yang sebenarnya jarang terlihat di video game market. Rata-rata game rogue-like itu dipenuhi dengan action, melee combat, atau aspek-aspek RPG. Ward of Fear merupakan eksperimen pertama untuk membuat game rogue-like yang memiliki hal-hal yang menyenangkan terkait game rogue-like dan juga ketegangan yang player dapatkan di game survival horror seperti Resident Evil atau The Evil Within. 

Game diawali di Day 1 dimana player harus mencari mesin teleporter untuk melarikan diri. Tetapi teleporter hanya dapat digunakan ketika passcode 7 digit sudah diketahui. Player harus mencari passcode tersebut dengan mengeksplor rumah sakit dan mudah-mudahan tidak dibunuh oleh salah satu hantunya.

How to Play:
WASD -> Move
Mouse Movement -> Aim
Left Mouse Button -> Attack
R -> Reload
Shift -> Run
Scroll Wheel / 1, 2, 3, 4 keys -> Switch Weapons
Tab -> Open/Close Inventory

Brief Game Logic Explanation:

Player
Player dapat bergerak dan berotasi berdasarkan mouse. Player dapat menyerang menggunakan 4 senjata. Pisau, Handgun, Shotgun, Rifle. Script player terpisah menjadi banyak yah. PlayerMovement, PlayerWeaponScript, PlayerAnimationControl, dll.
Player dapat lari hingga lelah dan akan ditunjukkin dengan berat nafasnya player. Ketika player lari, senjata tidak dapat digunakan. 

Inventory
Inventory system bekerja secara simple. Ia memiliki List class object Item yang distore secara otomatis dan setiap kali player menembak atau menggunakan resources, mereka akan baca melalui inventorysystem script. Item dapat di-drop atau digunakan ketika player membuka tas mereka.

UI
UI Ward of Fear itu simple. Di bawah kanan terdapat ammo counter. Dimana sebelah kiri adalah jumlah ammo yang ada di magazine senjata sedangkan yang di kanan adalah jumlah ammo yang ada di inventory untuk senjata tersebut. 
Ketika player terkena damage, layar akan memiliki efek merah dan semakin merah efek tersebut, semakin terlukanya player karakter.
Hampir segala aspek UI terdapat di script UIManager.

Game Manager
Script ini merupakan backbone Ward of Fear. Dimana segala aspek spawning musuh, loot, teleporter, note, player dilakukan. Game Manager juga bertanggung jawab atas saving dan loading data. Terdapat variable int SpawnerRank yang menentukan berapa banyak musuh dan loot yang akan dispawn pada map.


Thank you
Have fun!!!








