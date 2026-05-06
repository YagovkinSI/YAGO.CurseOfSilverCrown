import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useEffect } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetMyUserQuery } from '../entities/MyUser';
import YagoButton from '../shared/YagoButton';

const MyQuestListPage: React.FC = () => {
  const navigate = useNavigate();
  const myUserDataResult = useGetMyUserQuery();

  const isLoading = myUserDataResult.isLoading;
  const error = myUserDataResult.error;

  useEffect(() => {
    if (!(myUserDataResult.data?.data != undefined)) {
      navigate('/registration');
    }
  }, [myUserDataResult, navigate]);

  const renderCard = () => {
    return (
      <YagoCard
        title={`Задачи`}
        image={`/assets/images/pictures/captain_hall.jpg`}
      >
        <Typography textAlign="justify" gutterBottom>
          Данные раздел ещё находится в разработке.
        </Typography>
        <YagoButton onClick={() => navigate(-1)} type='secondary' >Закрыть</YagoButton>
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

export default MyQuestListPage