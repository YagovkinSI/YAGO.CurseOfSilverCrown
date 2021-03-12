import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import YagoButton from '../shared/YagoButton';
import { useNavigate } from 'react-router-dom';
import { useCreateTemporaryUserMutation, useGetQuery } from '../entities/MyUser';

const HomePage: React.FC = () => {
  const navigate = useNavigate();
  const myUserDataResult = useGetQuery();
  const [createTemporaryUser, createTemporaryUserResult] = useCreateTemporaryUserMutation();

  const isLoading = myUserDataResult.isLoading || createTemporaryUserResult.isLoading;
  const error = myUserDataResult.error ?? createTemporaryUserResult.error;

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
          {myUserDataResult.data!.data!.userName}, твои владения ждут своего правителя.
        </Typography>
        <ButtonWithLink to={'/game'} text={'Продолжить игру'} />
        {
          myUserDataResult.data!.data!.isTemporary
          && <ButtonWithLink to={'/registration'} text={'Изменить имя/пароль'} />
        }
      </>
    )
  }

  const renderCard = () => {
    const isAuthorized = myUserDataResult?.data?.isAuthorized;

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