import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import { useGetCurrentChapterQuery, useSetChoiceMutation, type StoryChoice } from '../entities/CurrentChapter';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetAuthorizationDataQuery } from '../entities/AuthorizationData';

const GamePage: React.FC = () => {
  const navigate = useNavigate();
  const authorizationData = useGetAuthorizationDataQuery();
  const currentChapterResult = useGetCurrentChapterQuery();
  const [currentIndex, setCurrentIndex] = useState<number>(currentChapterResult.data?.currentSlideIndex ?? 0);
  const [setChoice, setChoiceResult] = useSetChoiceMutation();

  const isLoading = currentChapterResult.isLoading || setChoiceResult.isLoading;
  const error = currentChapterResult.error ?? setChoiceResult.error;

  useEffect(() => {
    if (!authorizationData?.data?.isAuthorized) {
      navigate('/registration');
    }
  }, [authorizationData, navigate]);
  
  useEffect(() => {
    setCurrentIndex(currentChapterResult.data!.currentSlideIndex);
  }, [currentChapterResult]);

  const handleChoice = async (number: number) => {
    await setChoice({
      storyNodeId: currentChapterResult.data!.currentFragmentId,
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
    const card = currentChapterResult.data!.slides[currentIndex]!;
    const isLastCard = currentChapterResult.data!.slides.length == currentIndex + 1;

    const hasVariants = isLastCard && currentChapterResult.data!.choices.length > 1;
    const hasBack = currentIndex > 0;
    const hasContinue = !isLastCard;
    const hasNoVariants = isLastCard && !hasVariants;

    return (
      <YagoCard
        title={currentChapterResult.data!.title}
        image={`/assets/images/pictures/${card.imageName ?? 'home'}.jpg`}
      >
        {card.text.map(t =>
          <Typography textAlign="justify" gutterBottom>
            {t}
          </Typography>
        )}
        {hasVariants && currentChapterResult.data!.choices.map(c => renderChoiceButton(c))}
        {hasBack && <YagoButton onClick={() => setCurrentIndex(currentIndex - 1)} text={'Назад'} isDisabled={false} />}
        {hasContinue && <YagoButton onClick={() => setCurrentIndex(currentIndex + 1)} text={'Далее'} isDisabled={false} />}
        {hasNoVariants && currentChapterResult.data!.choices.map(c => renderChoiceButton(c))}
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