import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useState } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React from 'react';
import { useSearchParams } from 'react-router-dom';
import { useGetStoryQuery } from '../entities/Story';

const StoryPage: React.FC = () => {
  const [currentIndex, setCurrentIndex] = useState<number>(0);
  const [searchParams] = useSearchParams();

  const id = searchParams.get('id')
  const storyId = parseInt(id || '0', 10);
  const storyResult = useGetStoryQuery(storyId);

  const isLoading = storyResult.isLoading;
  const error = storyResult.error;

  const renderCard = () => {
    const card = storyResult.data!.slides[currentIndex]!;
    const isLastCard = storyResult.data!.slides.length == currentIndex + 1;

    const hasBack = currentIndex > 0;
    const hasContinue = !isLastCard;

    return (
      <YagoCard
        title={storyResult.data!.title}
        image={`/assets/images/pictures/${card.imageName ?? 'home'}.jpg`}
      >
        {card.text.map(t =>
          <Typography textAlign="justify" gutterBottom>
            {t}
          </Typography>
        )}
        {hasBack && <YagoButton onClick={() => setCurrentIndex(currentIndex - 1)} text={'Назад'} isDisabled={false} />}
        {hasContinue && <YagoButton onClick={() => setCurrentIndex(currentIndex + 1)} text={'Далее'} isDisabled={false} />}
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

export default StoryPage