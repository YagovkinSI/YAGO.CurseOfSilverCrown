import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useState } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React from 'react';
import type { Slide } from '../entities/Slide';
import { useNavigate } from 'react-router-dom';

const PrologPage: React.FC = () => {
  const navigate = useNavigate();
  const slides : Slide[] = [
    { id: 1, imageName: 'future_station', text: [
        'Галактика изменилась. Национальные государства пали под натиском корпораций. Теперь истинная власть — не на планетах, а среди звёзд. Валюта нового времени — суверенитет. Ваш корабль — это ваше законное владение, ваша крепость и ваш единственный шанс оставить след в истории. Добро пожаловать в эру частных космических городов.'
    ] },
    { id: 2, imageName: 'ship_1', text: [
        '"Рассвет-782" — ваш. Серийный, неказистый, но полностью функциональный корабль-город. Его ангары пусты, а жилые секторы молчат. Это ваш главный и единственный актив. Отныне вы — правитель, главнокомандующий и верховный судья в одном лице. Ваше правление начинается с чистого листа.'
    ] },
    { id: 3, imageName: 'empty_hangar', text: [
        'Впереди — ключевые решения. Вам предстоит определить законы, которые станут фундаментом вашего общества. Привлечь первых колонистов, готовых работать ради будущего. Создать промышленность, что наполнит казну. И, наконец, заработать репутацию, которая привлечёт в ваш космический город лучших специалистов и самые выгодные контракты.', 
        'Ваш путь начинается сейчас.'
    ] },
  ];
  const [currentIndex, setCurrentIndex] = useState<number>(0);

  const isLoading = false;
  const error = undefined;

  const renderCard = () => {
    const card = slides[currentIndex]!;
    const isLastCard = slides.length == currentIndex + 1;

    const hasBack = currentIndex > 0;
    const hasContinue = !isLastCard;

    return (
      <YagoCard
        title={`Пролог`}
        image={`/assets/images/pictures/${card.imageName ?? 'home'}.jpg`}
      >
        {card.text.map(t =>
          <Typography textAlign="justify" gutterBottom>
            {t}
          </Typography>
        )}
        {hasBack && <YagoButton onClick={() => setCurrentIndex(currentIndex - 1)} text={'Назад'} isDisabled={false} />}
        {hasContinue && <YagoButton onClick={() => setCurrentIndex(currentIndex + 1)} text={'Далее'} isDisabled={false} />}
        {isLastCard && <YagoButton onClick={() => navigate('/state')} text={'Начать правление'} isDisabled={false} />}
      </YagoCard>
    )
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : error != undefined
          ? <DefaultErrorCard />
          : renderCard()}
    </>
  )
}

export default PrologPage