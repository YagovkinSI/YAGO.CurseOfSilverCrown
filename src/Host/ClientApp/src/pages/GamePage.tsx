import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import { useAutoRegisterMutation, useGetCurrentUserQuery, useSetChoiceMutation } from '../entities/ApiEndpoints';
import type { StoryChoice } from '../entities/StoryNode';

const GamePage: React.FC = () => {
  const [currentIndex, setCurrentIndex] = useState<number>(0);
  const currentUserResult = useGetCurrentUserQuery();
  const [setChoice, setChoiceResult] = useSetChoiceMutation();
  const [autoRegister, autoRegisterResult] = useAutoRegisterMutation();

  const storyNode = setChoiceResult?.data ?? autoRegisterResult?.data?.storyNode ?? currentUserResult?.data?.storyNode;
  const isLoading = currentUserResult.isLoading || setChoiceResult.isLoading || autoRegisterResult.isLoading;
  const error = currentUserResult.error ?? setChoiceResult.error ?? autoRegisterResult.error;

  useEffect(() => {
    if (!currentUserResult.isLoading && !currentUserResult.error && !currentUserResult.data?.isAuthorized) {
      autoRegister({});
    }
  }, [currentUserResult, autoRegister]);

  const handleChoice = async (number: number) => {
    await setChoice({
      storyNodeId: storyNode!.id,
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
        key={choice.number}
        onClick={() => sendChoice(choice.number)}
        text={choice.text}
        isDisabled={false} />
    )
  }

  const renderCard = () => {
    const card = storyNode!.cards.find(c => c.number == currentIndex)!;
    const isLastCard = storyNode!.cards.length == currentIndex + 1;

    return (
      <YagoCard
        title={storyNode!.title}
        image={`/assets/images/pictures/${card.imageName ?? 'home'}.jpg`}
      >
        {card.text.map(t => 
          <Typography textAlign="justify" gutterBottom>
            {t}
          </Typography>
        )}
        {isLastCard && storyNode!.choices.map(c => renderChoiceButton(c))}
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
        : error != undefined
          ? <DefaultErrorCard />
          : renderCard()}
    </>
  )
}

export default GamePage