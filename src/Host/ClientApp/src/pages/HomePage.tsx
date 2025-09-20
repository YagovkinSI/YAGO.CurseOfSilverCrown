import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import YagoButton from '../shared/YagoButton';
import { useNavigate } from 'react-router-dom';
import { useAutoRegisterMutation, useGetAuthorizationDataQuery } from '../entities/AuthorizationData';

const HomePage: React.FC = () => {
  const navigate = useNavigate();
  const authorizationData = useGetAuthorizationDataQuery();
  const [autoRegister, autoRegisterResult] = useAutoRegisterMutation();

  const isLoading = authorizationData.isLoading || autoRegisterResult.isLoading;
  const error = authorizationData.error ?? autoRegisterResult.error;

  const autoRegisterAndGame = () => {
    autoRegister({})
      .unwrap()
      .then(() => navigate('/game'));
  }

  const renderGuestContent = () => {
    return (
      <>
        <Typography textAlign="justify" gutterBottom>
          Пройдите авторизацию или начните игру с временным аккаунтом.
        </Typography>
        <ButtonWithLink to={'/registration'} text={'Авторизация'} />
        <YagoButton onClick={autoRegisterAndGame} text={'Игра'} isDisabled={false} />
      </>
    )
  }
  
  const renderContinueStoryContent = () => {
    return (
      <>
        <Typography textAlign="justify" gutterBottom>
          Продолжите вашу историю, {authorizationData.data!.user!.userName}!?
        </Typography>
        <ButtonWithLink to={'/game'} text={'Продолжить игру'} />
        <ButtonWithLink to={'/registration'} text={'Изменить имя/пароль'} />
      </>
    )
  }

  const renderCard = () => {
    const isAuthorized = authorizationData?.data?.isAuthorized;

    return (
      <YagoCard
        title={`Yago World`}
        image={'/assets/images/pictures/home.jpg'}
      >
        {isAuthorized
          ? renderContinueStoryContent()
          : renderGuestContent()}
      </YagoCard>
    )
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : renderCard()}
    </>
  )
}

export default HomePage