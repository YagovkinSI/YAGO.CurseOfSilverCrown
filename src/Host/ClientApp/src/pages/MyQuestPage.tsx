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
import { useGetColonyQuestQuery, type MyQuest } from '../entities/MyQuest';
import TextMain from '../shared/TextMain';
import type { ColonyParameter } from '../entities/ColonyParameter';
import ColonyParameterList from '../features/ColonyParameterList';
import YagoCardContentInputField from '../shared/YagoCardContentInputField';
import { SanitizeColonyName, ValidateColonyName } from '../features/ColonyNameValidator';

const MyQuestPage: React.FC = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const myUserDataResult = useGetMyUserQuery();
  const colonyQuestResult = useGetColonyQuestQuery(id ?? "");
  const [inputTextValue, setInputTextValue] = useState('');
  const [inputTextError, setInputTextError] = useState('');

  const isLoading = myUserDataResult.isLoading || colonyQuestResult.isLoading;
  const error = myUserDataResult.error ?? colonyQuestResult.error;

  useEffect(() => {
    if (!(myUserDataResult.data?.data != undefined)) {
      navigate('/registration');
    }
  }, [myUserDataResult, navigate]);

  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));

  const validateInputText = (value: string): boolean => {
    const validationResult = ValidateColonyName(value);
    if (!validationResult.isValid) {
      setInputTextError(validationResult.error!);
      return false;
    }
    setInputTextError('');
    return true;
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

  const handleQuestResult = async (quest: MyQuest) => {
    if (quest.prologueSlide.textInput != undefined) {
      setInputTextValue(SanitizeColonyName(inputTextValue));
      const validationResult = ValidateColonyName(inputTextValue);
      if (!validationResult.isValid) {
        setInputTextError(validationResult.error!);
      }
    }
    navigate(`/me/quest/complete/${id}/${inputTextValue}`)
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

  const renderCard = (quest: MyQuest) => {
    return (
      <YagoCard
        title={quest.prologueSlide.title}
        image={`/assets/images/pictures/${quest.prologueSlide.imageName}.jpg`}
      >
        <TextMain textArray={quest.prologueSlide.text} />
        {renderParameters(quest.prologueSlide.parameters)}
        {quest.prologueSlide.textInput != undefined
          && <YagoCardContentInputField value={inputTextValue} label='Название колонии' handleChange={handleInputTextChange} error={inputTextError} />}
        <YagoButton onClick={() => handleQuestResult(quest)} isDisabled={!quest.completed} >{quest.prologueSlide.continueButtonName}</YagoButton>
        <YagoButton onClick={() => navigate(-1)} type='secondary'>Закрыть</YagoButton>
      </YagoCard>
    )
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading || colonyQuestResult.data == undefined
        ? <LoadingCard />
        : error != undefined || colonyQuestResult.data.data == undefined
          ? <DefaultErrorCard />
          : renderCard(colonyQuestResult.data.data)}
    </>
  )
}

export default MyQuestPage