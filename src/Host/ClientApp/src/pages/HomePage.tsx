import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import YagoButton from '../shared/YagoButton';
import { useNavigate } from 'react-router-dom';
import { useAutoRegisterMutation, useGetCurrentUserQuery } from '../entities/CurrentUser';
import { useDropStoryMutation, useGetCurrentFragmentQuery } from '../entities/CurrentStoryNode';

const HomePage: React.FC = () => {
  const navigate = useNavigate();
  const currentUserResult = useGetCurrentUserQuery();
  const currentFragmentResult = useGetCurrentFragmentQuery(undefined, { skip: !currentUserResult?.data?.isAuthorized });
  const [autoRegister, autoRegisterResult] = useAutoRegisterMutation();
  const [dropStory, dropStoryResult] = useDropStoryMutation();

  const isLoading = currentUserResult.isLoading || autoRegisterResult.isLoading || currentFragmentResult.isLoading || dropStoryResult.isLoading;
  const error = currentUserResult.error ?? autoRegisterResult.error ?? currentFragmentResult.error ?? dropStoryResult.error;
  
  const autoRegisterAndGame = () => {
    autoRegister({})
      .unwrap()
      .then(() => navigate('/game'));
  }

  const sendDropStory = () => {
    dropStory({});
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
  const renderNewStoryContent = () => {
    return (
      <>
        <Typography textAlign="justify" gutterBottom>
          Начните своё приключение, {currentUserResult.data!.user!.userName}!
        </Typography>
        <ButtonWithLink to={'/game'} text={'Игра'} />
        <ButtonWithLink to={'/registration'} text={'Изменить имя/пароль'} />
      </>
    )
  }
  const renderContinueStoryContent = () => {
    return (
      <>
        <Typography textAlign="justify" gutterBottom>
          Продолжите вашу историю, {currentUserResult.data!.user!.userName}!?
        </Typography>
        <ButtonWithLink to={'/game'} text={'Продолжить игру'} />
        <YagoButton onClick={() => sendDropStory()} text={'Удалить сохранения'} isDisabled={false} />
        <ButtonWithLink to={'/registration'} text={'Изменить имя/пароль'} />
      </>
    )
  }

  const renderCard = () => {
    const isAuthorized = currentUserResult?.data?.isAuthorized;
    const hasProgress = isAuthorized && (currentFragmentResult?.data?.id ?? 0) > 0

    return (
      <YagoCard
        title={`Yago World`}
        image={'/assets/images/pictures/home.jpg'}
      >
        {isAuthorized
          ? hasProgress
            ? renderContinueStoryContent()
            : renderNewStoryContent()
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