import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { useGetCurrentStoryQuery, useSetChoiceMutation, type StoryChoice } from '../entities/StoryNode';
import { useGetCurrentUserQuery } from '../entities/CurrentUser';
import YagoButton from '../shared/YagoButton';

const GamePage: React.FC = () => {
  const navigate = useNavigate();
  const [currentIndex, setCurrentIndex] = useState<number>(0);
  const currentUserResult = useGetCurrentUserQuery();
  const currentStoryResult = useGetCurrentStoryQuery();
  const [setChoice] = useSetChoiceMutation();

  const isLoading = currentUserResult.isLoading || currentStoryResult.isLoading;
  const error = currentUserResult.error ?? currentStoryResult.error;

  useEffect(() => {
    if (!currentUserResult.isLoading && !currentUserResult.error && !currentUserResult.data?.isAuthorized) {
      navigate('/registration');
    }
  }, [currentUserResult, navigate]);

  const handleChoice = async (number: number) => {
    try {
      const result = await setChoice({
        storyNodeId: currentStoryResult.data!.id,
        choiceNumber: number
      }).unwrap();
      console.log('Успешно:', result);
    } catch (error) {
      console.error('Ошибка:', error);
    }
  }

  const sendChoice = (number: number) => {
    currentStoryResult.isLoading = true;
    setCurrentIndex(0);
    handleChoice(number);
  }

  const renderChoiceButton = (choice: StoryChoice) => {
    return (
      <YagoButton
        key={choice.number}
        onClick={() => sendChoice(choice.number)}
        text={choice.text}
        isDisabled={false} />
    )
  }

  const renderCard = () => {
    const card = currentStoryResult.data!.cards.find(c => c.number == currentIndex)!;
    const isLastCard = currentStoryResult.data!.cards.length == currentIndex + 1;

    return (
      <YagoCard
        title={currentStoryResult.data!.title}
        image={`/assets/images/pictures/${card.imageName ?? 'home'}.jpg`}
      >
        <Typography textAlign="justify" gutterBottom>
          {card.text}
        </Typography>
        {isLastCard && currentStoryResult.data!.choices.map(c => renderChoiceButton(c))}
        {currentIndex > 0 && <YagoButton onClick={() => setCurrentIndex(currentIndex - 1)} text={'Назад'} isDisabled={false} />}
        {!isLastCard && <YagoButton onClick={() => setCurrentIndex(currentIndex + 1)} text={'Далее'} isDisabled={false} />}
      </YagoCard>
    )
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : error != undefined || currentStoryResult.data == undefined
          ? <DefaultErrorCard />
          : renderCard()}
    </>
  )
}

export default GamePage