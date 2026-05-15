import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useEffect } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useGetMyUserQuery } from '../entities/MyUser';
import YagoButton from '../shared/YagoButton';
import { useGetColonyQuestQuery, type ColonyQuest } from '../entities/ColonyQuest';

const MyQuestPage: React.FC = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const myUserDataResult = useGetMyUserQuery();
  const colonyQuestResult = useGetColonyQuestQuery(id ?? "");

  const isLoading = myUserDataResult.isLoading || colonyQuestResult.isLoading;
  const error = myUserDataResult.error ?? colonyQuestResult.error;

  useEffect(() => {
    if (!(myUserDataResult.data?.data != undefined)) {
      navigate('/registration');
    }
  }, [myUserDataResult, navigate]);

  const renderCard = (quest: ColonyQuest) => {
    return (
      <YagoCard
        title={quest.name}
        image={`/assets/images/pictures/homepage.jpg`}
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
      {isLoading || colonyQuestResult.data == undefined
        ? <LoadingCard />
        : error != undefined || colonyQuestResult.data.data == undefined
          ? <DefaultErrorCard />
          : renderCard(colonyQuestResult.data.data)}
    </>
  )
}

export default MyQuestPage