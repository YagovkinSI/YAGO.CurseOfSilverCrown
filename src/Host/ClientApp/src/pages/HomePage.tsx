import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { Typography } from '@mui/material';
import { useAutoRegisterMutation, useGetAuthorizationDataQuery } from '../entities/AuthorizationData';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';

const HomePage: React.FC = () => {
  const navigate = useNavigate();
  const authorizationDataResult = useGetAuthorizationDataQuery();
  const [autoRegister, autoRegisterResult] = useAutoRegisterMutation();

  const isLoading = authorizationDataResult.isLoading || autoRegisterResult.isLoading;// || playthrough.isLoading || dropPlaythroughResult.isLoading;
  const error = authorizationDataResult.error ?? autoRegisterResult.error;// ?? playthrough.error ?? dropPlaythroughResult.error;

  const autoRegisterAndGame = () => {
    autoRegister({})
      .unwrap()
      .then(() => navigate('/app'));
  }

  const renderGuestContent = () => {
    return (
      <>
        <Typography textAlign="center" gutterBottom>
          Хитрость, сила или дипломатия? Выбери путь к власти.
        </Typography>
        <YagoButton onClick={autoRegisterAndGame} text={'Начать игру'} isDisabled={false} />
        <ButtonWithLink to={'/app/registration'} text={'Авторизация'} />
      </>
    )
  }

  const renderUserContent = () => {
    return (
      <>
        <Typography textAlign="center" gutterBottom>
          {authorizationDataResult.data!.user!.userName}, твои владения ждут своего правителя.
        </Typography>
        {
          authorizationDataResult.data?.faction != undefined
            ? <ButtonWithLink to={'/Domain'} text={'К владению'} />
            : <ButtonWithLink to={'/app/map'} text={'Выбрать владение на карте'} />
        }
      </>
    )
  }

  const renderCard = () => {
    return (
      <YagoCard
        title={'Yago World'}
        image={'/assets/images/pictures/homepage.jpg'}
        headerButtonsAccess={false}
      >
        {authorizationDataResult.data?.isAuthorized
          ? renderUserContent()
          : renderGuestContent()}
      </YagoCard>
    )
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : error == undefined && authorizationDataResult.data != undefined
          ? renderCard()
          : <DefaultErrorCard />}
    </>
  )
}

export default HomePage