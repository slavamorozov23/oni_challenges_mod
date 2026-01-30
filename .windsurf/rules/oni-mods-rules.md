---
trigger: always_on
---

"mods\NoPollutionMod" наш мод
"repos" репозитории с целой библиотекой модов, например "repos\romen-h_ONI-Mods\src\PlasticDoor" мод на пластиковые двери
"readable\Assembly-CSharp" dnSpy дамп, Assembly-CSharp.dll игры
"tools\kanim-explorer" - просмотр и парвка анимаций ONI в формате .kanim
"tools\kanimal-SE"- библиотека, cli конвертер, между форматами spriter->klei kanim и обратно

для проверки нашего мода на ошибки линтера, команда -
"& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" `
  "C:\Users\SlavaMorozov\Documents\ONI-Modding\ONI-Modding.sln" `
  /t:Build /p:Configuration=Debug"
для исправления списка референсов dll -
"mods\NoPollutionMod\NoPollutionMod.csproj"
доступные референсы dll для проекта, подключаются из -
"refs\oni_managed"