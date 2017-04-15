# Generator

## Описание

Generator (Reinforcement learning) - демонстрационный проект работы нейросетей по классификации сигналов на три категории Long, Short, Wait с целью максимизации средного дохода за день.

![1](https://cloud.githubusercontent.com/assets/26124790/25063606/d8c70db0-21f1-11e7-9f2e-896d8e31eece.png)

### Алгоритм:
Для примера выбраны данные по фьючерсу на индекс РТС (данные поссылке [Google Drive](https://drive.google.com/open?id=0B-sgEGteHltAS3J5VlNTeUpEcmM) или скачать с [Finam](https://www.finam.ru/profile/mosbirzha-fyuchersy/rts/export/?market=14&em=17455&code=SPFB.RTS&apply=0&df=3&mf=2&yf=2017&from=03.03.2017&dt=3&mt=2&yt=2017&to=03.03.2017&p=7&f=SPFB.RTS_170303_170303&e=.txt&cn=SPFB.RTS&dtf=1&tmf=1&MSOR=1&mstime=on&mstimever=1&sep=1&sep2=1&datf=1&at=1))

1. Парсинг .txt и построение Days из тиков.
2. Построение Signals в формате SMA(40), M5, M1, T150(свечи 150 тиков).
3. На вход сети получают три последних сигнала и методом e-greedy выбирается действие: покупка(Long), продажа(Short), пропустить сигнал (Wait).
4. Обучение сетей методом SGD.

![genratorgif](https://cloud.githubusercontent.com/assets/26124790/25063757/93e9e222-21f5-11e7-9199-ebeb2e69d3dc.gif)

## Дополнительно

Take/Stop уровнень (250 пунктов по умолчанию) Для изменения Settings => Slider и Refresh(обнулить все параметры).



