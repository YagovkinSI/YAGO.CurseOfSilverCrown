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
import { QuestType, useCompleteQuestMutation, useGetColonyQuestQuery } from '../entities/MyQuest';
import TextMain from '../shared/TextMain';
import type { ColonyParameter } from '../entities/ColonyParameter';
import ColonyParameterList from '../features/ColonyParameterList';
import YagoCardContentInputField from '../shared/YagoCardContentInputField';
import { SanitizeColonyName, ValidateColonyName } from '../features/ColonyNameValidator';
import type { Episode, SlideButton, SlideButtonAction } from '../entities/Episode';

const MyQuestPage: React.FC = () => {
  const { id } = useParams();
  const [slideIndex, setSlideIndex] = useState<number>(0);
  const navigate = useNavigate();
  const myUserDataResult = useGetMyUserQuery();
  const colonyQuestResult = useGetColonyQuestQuery(id ?? "");
  const [completeQuestMutation, completeQuestResult] = useCompleteQuestMutation();
  const [inputTextValue, setInputTextValue] = useState('');
  const [inputTextError, setInputTextError] = useState('');
  const [handleChoiceError, setHandleChoiceError] = useState<string | undefined>(undefined);

  const isLoading = myUserDataResult.isLoading || colonyQuestResult.isLoading;
  const error = myUserDataResult.error ?? colonyQuestResult.error ?? handleChoiceError;
  const episode = completeQuestResult.data ?? colonyQuestResult.data?.data?.episode;
  const canBeClosed = completeQuestResult.data != undefined || colonyQuestResult.data?.data?.type != QuestType.Immediately;
  console.log('episode', episode)

  useEffect(() => {
    if (!(myUserDataResult.data?.data != undefined)) {
      navigate('/registration');
    }
  }, [myUserDataResult, navigate]);

  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));

  const handleSetSlideId = (slideId: string) => {
    const index = episode?.slides?.findIndex(x => x.id == slideId);
    if (index == undefined)
      return;
    setSlideIndex(index);
  };

  const handleSetChoice = async (action: SlideButtonAction, inputTextValue?: string | undefined) => {
    try {
      const result = await completeQuestMutation({ id: action.arguments[0], dilemmaResolving: inputTextValue ?? action.arguments[1] }).unwrap();
      if (result.slides.length == 0)
        navigate('/me/colony');
    } catch (e) {
      if (e && typeof e === 'object' && 'data' in e) {
        const errorData = (e as { data?: { title?: string } }).data;
        setHandleChoiceError(errorData?.title ?? 'Неизвестная ошибка.');
      } else {
        setHandleChoiceError('Неизвестная ошибка.');
      }
    }
  };

  const handleInputTextSave = async (action: SlideButtonAction) => {
    setInputTextValue(SanitizeColonyName(inputTextValue));
    const validationResult = ValidateColonyName(inputTextValue);
    if (!validationResult.isValid) {
      setInputTextError(validationResult.error!);
    }
    else {
      handleSetChoice(action, inputTextValue);
    }
  };

  const handleInputTextChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setInputTextValue(value);
    if (value.length > 2) {
      validateInputText(value);
    } else {
      setInputTextError('');
    }
  };

  const validateInputText = (value: string): boolean => {
    const validationResult = ValidateColonyName(value);
    if (!validationResult.isValid) {
      setInputTextError(validationResult.error!);
      return false;
    }
    setInputTextError('');
    return true;
  };

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

  const renderSlideButton = (button: SlideButton, withTextInput: boolean) => {
    const isMutation = button.action != undefined;
    const onClick = button.action != undefined
      ? withTextInput
        ? () => handleInputTextSave(button.action!)
        : () => handleSetChoice(button.action!)
      : button.navigate != undefined
        ? () => navigate(button.navigate!.actionUrl)
        : button.toSlide != undefined
          ? () => handleSetSlideId(button.toSlide!.slideId)
          : () => { };

    return (
      <YagoButton type={isMutation ? 'mutation' : 'navigation'} onClick={onClick} isDisabled={!button.isAvailable}>
        {button.name}
      </YagoButton>)
  }

  const renderCard = (episode: Episode, canBeClosed: boolean) => {
    const slide = episode.slides[slideIndex];
    return (
      <YagoCard
        title={slide.title}
        image={`/assets/images/pictures/${slide.imageName}.jpg`}
      >
        <TextMain textArray={slide.text} />
        {renderParameters(slide.parameters)}
        {slide.textInput != undefined && <YagoCardContentInputField value={inputTextValue} label='Название колонии' handleChange={handleInputTextChange} error={inputTextError} />}
        {slide.buttons.map(x => renderSlideButton(x, slide.textInput != undefined))}
        {canBeClosed && <YagoButton onClick={() => navigate(-1)} type='secondary'>Закрыть</YagoButton>}
      </YagoCard>
    )
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading || episode == undefined
        ? <LoadingCard />
        : error != undefined
          ? <DefaultErrorCard />
          : renderCard(episode!, canBeClosed)}
    </>
  )
}

export default MyQuestPage