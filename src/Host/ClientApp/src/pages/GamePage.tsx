import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import { useGetPlaythroughQuery, useSetChoiceMutation, type StoryChoice } from '../entities/Playthrough';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetAuthorizationDataQuery } from '../entities/AuthorizationData';

const GamePage: React.FC = () => {
  const navigate = useNavigate();
  const authorizationData = useGetAuthorizationDataQuery();
  const playthrough = useGetPlaythroughQuery();
  const [currentIndex, setCurrentIndex] = useState<number>(playthrough.data?.currentSlideIndex ?? 0);
  const [setChoice, setChoiceResult] = useSetChoiceMutation();

  const isLoading = playthrough.isLoading || setChoiceResult.isLoading;
  const error = playthrough.error ?? setChoiceResult.error;

  useEffect(() => {
    if (!authorizationData?.data?.isAuthorized) {
      navigate('/registration');
    }
  }, [authorizationData, navigate]);
  
  useEffect(() => {
    setCurrentIndex(playthrough.data!.currentSlideIndex);
  }, [playthrough]);

  const handleChoice = async (number: number) => {
    await setChoice({
      storyNodeId: playthrough.data!.currentFragmentId,
      choiceNumber: number
    });
  }

  const sendChoice = (number: number) => {
    handleChoice(number);
  }

  const renderChoiceButton = (choice: StoryChoice) => {
    return (
      <YagoButton
        key={choice.fragmentId}
        onClick={() => sendChoice(choice.fragmentId)}
        text={choice.text}
        isDisabled={false} />
    )
  }

  const renderCard = () => {
    const card = playthrough.data!.slides[currentIndex]!;
    const isLastCard = playthrough.data!.slides.length == currentIndex + 1;

    const hasVariants = isLastCard && playthrough.data!.choices.length > 1;
    const hasBack = currentIndex > 0;
    const hasContinue = !isLastCard;
    const hasNoVariants = isLastCard && !hasVariants;

    return (
      <YagoCard
        title={playthrough.data!.title}
        image={`/assets/images/pictures/${card.imageName ?? 'home'}.jpg`}
      >
        {card.text.map(t =>
          <Typography textAlign="justify" gutterBottom>
            {t}
          </Typography>
        )}
        {hasVariants && playthrough.data!.choices.map(c => renderChoiceButton(c))}
        {hasBack && <YagoButton onClick={() => setCurrentIndex(currentIndex - 1)} text={'Назад'} isDisabled={false} />}
        {hasContinue && <YagoButton onClick={() => setCurrentIndex(currentIndex + 1)} text={'Далее'} isDisabled={false} />}
        {hasNoVariants && playthrough.data!.choices.map(c => renderChoiceButton(c))}
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

export default GamePage