import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import { useGetCurrentFragmentQuery, useSetChoiceMutation, type StoryChoice } from '../entities/CurrentStoryNode';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetCurrentUserQuery } from '../entities/CurrentUser';

const GamePage: React.FC = () => {
  const navigate = useNavigate();
  const [currentIndex, setCurrentIndex] = useState<number>(0);
  const currentUserResult = useGetCurrentUserQuery();
  const currentFragmentResult = useGetCurrentFragmentQuery();
  const [setChoice, setChoiceResult] = useSetChoiceMutation();

  const isLoading = currentFragmentResult.isLoading || setChoiceResult.isLoading;
  const error = currentFragmentResult.error ?? setChoiceResult.error;

  useEffect(() => {
    if (!currentUserResult?.data?.isAuthorized) {
      navigate('/registration');
    }
  }, [currentUserResult, navigate]);

  const handleChoice = async (number: number) => {
    await setChoice({
      storyNodeId: currentFragmentResult.data!.id,
      choiceNumber: number
    });
  }

  const sendChoice = (number: number) => {
    setCurrentIndex(0);
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
    const card = currentFragmentResult.data!.slides[currentIndex]!;
    const isLastCard = currentFragmentResult.data!.slides.length == currentIndex + 1;

    const hasVariants = isLastCard && currentFragmentResult.data!.choices.length > 1;
    const hasBack = currentIndex > 0;
    const hasContinue = !isLastCard;
    const hasNoVariants = isLastCard && !hasVariants;

    return (
      <YagoCard
        title={currentFragmentResult.data!.title}
        image={`/assets/images/pictures/${card.imageName ?? 'home'}.jpg`}
      >
        {card.text.map(t =>
          <Typography textAlign="justify" gutterBottom>
            {t}
          </Typography>
        )}
        {hasVariants && currentFragmentResult.data!.choices.map(c => renderChoiceButton(c))}
        {hasBack && <YagoButton onClick={() => setCurrentIndex(currentIndex - 1)} text={'Назад'} isDisabled={false} />}
        {hasContinue && <YagoButton onClick={() => setCurrentIndex(currentIndex + 1)} text={'Далее'} isDisabled={false} />}
        {hasNoVariants && currentFragmentResult.data!.choices.map(c => renderChoiceButton(c))}
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