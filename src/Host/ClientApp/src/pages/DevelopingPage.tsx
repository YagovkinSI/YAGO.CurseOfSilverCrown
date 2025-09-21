import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useEffect } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetAuthorizationDataQuery } from '../entities/AuthorizationData';

const DevelopingPage: React.FC = () => {
  const navigate = useNavigate();
  const authorizationData = useGetAuthorizationDataQuery();

  const isLoading = authorizationData.isLoading;
  const error = authorizationData.error;

  useEffect(() => {
    if (!authorizationData?.data?.isAuthorized) {
      navigate('/registration');
    }
  }, [authorizationData, navigate]);

  const renderCard = () => {
    return (
      <YagoCard
        title={`В разработке`}
        image={`/assets/images/pictures/homepage.jpg`}
      >
        <Typography textAlign="justify" gutterBottom>
          Данные раздел ещё находится в разработке.
        </Typography>
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

export default DevelopingPage