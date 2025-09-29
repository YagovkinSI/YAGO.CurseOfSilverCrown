import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import YagoButton from '../shared/YagoButton';
import { useNavigate } from 'react-router-dom';
import { useCreateTemporaryUserMutation, useGetAuthorizationDataQuery } from '../entities/AuthorizationData';

const HomePage: React.FC = () => {
  const navigate = useNavigate();
  const authorizationData = useGetAuthorizationDataQuery();
  const [createTemporaryUser, createTemporaryUserResult] = useCreateTemporaryUserMutation();

  const isLoading = authorizationData.isLoading || createTemporaryUserResult.isLoading;
  const error = authorizationData.error ?? createTemporaryUserResult.error;

  const autoRegisterAndGame = () => {
    createTemporaryUser({})
      .unwrap()
      .then(() => navigate('/game'));
  }

  const renderGuestContent = () => {
    return (
      <>
        <Typography textAlign="justify" gutterBottom>
          Хитрость, сила или дипломатия? Выбери путь к власти.
        </Typography>
        <YagoButton onClick={autoRegisterAndGame} text={'Начать игру'} isDisabled={false} />
        <ButtonWithLink to={'/registration'} text={'Авторизация'} />
      </>
    )
  }
  
  const renderContinueStoryContent = () => {
    return (
      <>
        <Typography textAlign="justify" gutterBottom>
          {authorizationData.data!.user!.userName}, твои владения ждут своего правителя.
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
        image={'/assets/images/pictures/homepage.jpg'}
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