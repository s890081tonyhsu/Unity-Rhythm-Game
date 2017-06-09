Music Rhythm Game
====

Introduction
----

This is a rhythm game created in unity with C#. The idea is from StepMania, which is the game I play when I was a high school student.

All the game need is to make a rhythm that fits the song needs. So we have song custom stage called beat map, it will follow the song's beats if the creator is good at finding the song's beats.

----

這是一個用Unity做的音樂遊戲，使用C#撰寫程式內容，靈感由來是我高中玩過的StepMania。

這種節奏遊戲的目標就是敲打出音樂中的節奏，而打節奏的關卡我們稱之為譜面。如果製譜者設計正常的話，搭配音樂會是一個完美的節奏打擊。

Features
----

- Reading StepMania's beatmap(.sm file)
- Support playing .ogg and .mp3 songfile
- Realtime generate beats note from beatmap(same as StepMania)
- Combo will make you to get higher score(same as StepMania)

----

- 讀取StepMania的譜面檔(.sm檔)
- 能夠播放ogg檔跟mp3檔
- 根據譜面動態產生節奏符號(跟StepMania一樣)
- 打擊連鎖可以獲得較多分數(跟StepMania一樣)

Interface & how to play
----

- Loading: loading the songs
- Menu: choosing the song and the difficulties
- Game: for playing
- Scoreboard: showing the score

Before you play, you should make sure there is a folder called "Songs", this folder will hold all the beatsmap that you want to choose to play.

After the loading is complete, you can find the songs and difficulties, choose one song by press left and right arrow key, one difficulty by press up and down arrow key, then press "Enter".

In the game, use "D" as left note, "F" as down note, "J" as up note and "K" as right note. Press the right button when the note is on the bottom of the screen, then you'll get the score.

At the end of the song, it will show your score, when you press "Enter", you can go back to the menu and choose another one.

----

- 讀取：用來遮蔽背景讀取譜面內容
- 選單：選擇歌曲跟難度
- 遊戲：遊戲中畫面
- 得分版：顯示你的分數

在遊戲之前，先確認根目錄下是否有一個叫做Songs的資料夾，那是用來放置遊戲用譜面的。

在遊戲讀取結束後，他會顯示出你有的歌曲跟譜面難度。使用左右選擇歌曲，上下選擇難度，按下Enter就可以開始玩了。

遊戲中你要使用D、F、J、K四個按鈕。在符號到達底端的時候按下對應的按鈕就可以得到分數。

在歌曲結束後，他會顯示你的分數，按下Enter之後就可以回到選單再選別首。

Technics
----

SM files is special because it writes song's tag and footsteps. We need to use finite state machine to make sure that the program is trying to read tag or footsteps.

And the footsteps is like we know in music class. It uses measure as we draw bars on footsteps. How we process quarter note, eighth note, sixteen note is to cut a bar into pieces, and the mark on the bar. We only need to mark the start of the note so that the player can make the right place to key.

There is a gameobject which holdes all songs and won't be destroyed when changing scene until you close the game.

Unity's UI doesn't have built in linear gradient color for text background. So I find a shader to show that effects.

On unity personal in windows, we can only play ogg(mp3 for android) file when we dynamic read the file. So I find a plugin to play mp3 file on windows.

----

SM檔具有歌曲的相關資料跟譜面，所以我們用狀態機來偵測我們是在讀歌曲資料還是譜面資料。

而譜面資料就跟音樂課上的五線譜一樣，五線譜上的每一行在檔案內被稱為measure。我們會把一行切成很多部分來標記音符，而標記只需要音符的開頭即可，這樣在遊戲中的玩家就可以敲對節拍。

我們有一個遊戲物件可以保管所有歌曲，他不會因為切換場景而被摧毀，直到你關閉遊戲。

Unity UI內建沒有對文字背景的漸層，所以我們找到一個現成的漸層來輔助。

Unity personal 在windows上只允許播放ogg(在android上只能播mp3)，所以我們找了一個插件來讓它支援在windows 上播放mp3。

Reference
----

- [How to create a Unity Rhythm Game Part 1: Parsing the .SM file](http://blog.phantombadger.com/2016/05/23/how-to-create-unity-rhythm-game-part-1-parsing-the-sm-file/)
- [How to create a Unity Rhythm Game Part 2: Generating the Steps](http://blog.phantombadger.com/2016/05/26/how-to-create-a-unity-rhythm-game-part-2-generating-the-steps/)