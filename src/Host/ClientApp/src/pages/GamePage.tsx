import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { useGetCurrentStoryQuery, type StoryChoice } from '../entities/StoryNode';
import YagoButton from '../shared/YagoButton';

const GamePage: React.FC = () => {
  const navigate = useNavigate();
  const [currentIndex, setCurrentIndex] = useState<number>(0);
  const { data, isLoading, error } = useGetCurrentStoryQuery();

  useEffect(() => {
    if (!isLoading && !error && data?.id == -1) {
      navigate('/Identity/Account/Register');
    }
  }, [data, isLoading, error, navigate]);

  const sendChoice = (number: number) => {
    console.log(number);
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
    const card = data!.cards.find(c => c.number == currentIndex)!;
    const isLastCard = data!.cards.length == currentIndex + 1;

    return (
      <YagoCard
        title={data!.title}
        image={`/assets/images/pictures/${card.imageName}.jpg`}
      >
        <Typography textAlign="justify" gutterBottom>
          {card.text}
        </Typography>
        {isLastCard
          ? data!.choices.map(c => renderChoiceButton(c))
          : <YagoButton onClick={() => setCurrentIndex(currentIndex + 1)} text={'Далее'} isDisabled={false} />
        }
      </YagoCard>
    )
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : error != undefined || data == undefined
          ? <DefaultErrorCard />
          : renderCard()}
    </>
  )
}

export default GamePage