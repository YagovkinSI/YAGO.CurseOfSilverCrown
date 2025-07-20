import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { useGetCurrentStoryQuery, useSetChoiceMutation, type StoryChoice } from '../entities/StoryNode';
import { useAutoRegisterMutation, useGetCurrentUserQuery } from '../entities/CurrentUser';
import YagoButton from '../shared/YagoButton';

const GamePage: React.FC = () => {
  const [currentIndex, setCurrentIndex] = useState<number>(0);
  const currentUserResult = useGetCurrentUserQuery();
  const currentStoryResult = useGetCurrentStoryQuery();
  const [setChoice] = useSetChoiceMutation();
  const [autoRegister] = useAutoRegisterMutation();

  const isLoading = currentUserResult.isLoading || currentStoryResult.isLoading;
  const error = currentUserResult.error ?? (currentUserResult.data?.isAuthorized ? currentStoryResult.error : undefined);

  useEffect(() => {
    if (!currentUserResult.isLoading && !currentUserResult.error && !currentUserResult.data?.isAuthorized) {
      autoRegister();
    }
  }, [currentUserResult]);

  const handleChoice = async (number: number) => {
    await setChoice({
      storyNodeId: currentStoryResult.data!.id,
      choiceNumber: number
    });
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
        {card.text.map(t => 
          <Typography textAlign="justify" gutterBottom>
            {t}
          </Typography>
        )}
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