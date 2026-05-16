import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
import { useEffect, useState } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useGetMyUserQuery } from '../entities/MyUser';
import YagoButton from '../shared/YagoButton';
import TextMain from '../shared/TextMain';
import type { ColonyParameter } from '../entities/ColonyParameter';
import ColonyParameterList from '../features/ColonyParameterList';
import { useCompleteQuestMutation } from '../entities/MyQuest';
import type { Episode, PrologueSlide } from '../entities/Episode';

const MyQuestCompletePage: React.FC = () => {
  const { id } = useParams();
  const [slideIndex, setSlideIndex] = useState<number>(0);
  const navigate = useNavigate();
  const myUserDataResult = useGetMyUserQuery();
  const [completeQuestMutation, completeQuestResult] = useCompleteQuestMutation();

  const isLoading = myUserDataResult.isLoading || completeQuestResult.isLoading;
  const error = myUserDataResult.error ?? completeQuestResult.error;

  useEffect(() => {
    completeQuestMutation({ id: id ?? '', dilemmaResolving: '' });
  }, [completeQuestMutation]);

  useEffect(() => {
    if (!(myUserDataResult.data?.data != undefined)) {
      navigate('/registration');
    }
  }, [myUserDataResult, navigate]);

  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));

  const renderParameters = (parameters: ColonyParameter[]) => {
    if (parameters.length == 0)
      return <></>

    return (
      <Box
        display="flex"
        flexDirection="column"
        gap={1}
        sx={{
          width: '100%',
          maxWidth: isMobile ? 350 : 700,
          margin: '0 auto'
        }}
      >
        <ColonyParameterList items={parameters} />
      </Box>
    )
  }

  const renderPrologueSlide = (slide: PrologueSlide, slideCount: number) => {
    return (
      <YagoCard
        title={slide.title}
        image={`/assets/images/pictures/${slide.imageName}.jpg`}
      >
        <TextMain textArray={slide.text} />
        {renderParameters(slide.parameters)}
        {slideIndex > 0 && <YagoButton onClick={() => setSlideIndex(slideIndex - 1)} type='secondary'>Назад</YagoButton>}
        {slideIndex < slideCount - 1 && <YagoButton onClick={() => setSlideIndex(slideIndex + 1)}>{slide.continueButtonName}</YagoButton>}
        {slideIndex == slideCount - 1 && renderCloseButton()}
      </YagoCard>
    )
  }

  const renderCloseButton = () => {
    return (
      <YagoButton onClick={() => navigate("/me/colony")} type='secondary'>Закрыть</YagoButton>
    )
  }

  const renderCard = (episode: Episode) => {
    return renderPrologueSlide(episode.prologueSlides[slideIndex], episode.prologueSlides.length);
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading || completeQuestResult.data == undefined
        ? <LoadingCard />
        : error != undefined
          ? <DefaultErrorCard />
          : renderCard(completeQuestResult.data)}
    </>
  )
}

export default MyQuestCompletePage