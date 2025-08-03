import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import YagoButton from '../shared/YagoButton';
import { useNavigate } from 'react-router-dom';
import { useAutoRegisterMutation, useGetAuthorizationDataQuery } from '../entities/AuthorizationData';
import { useDropStoryMutation, useGetCurrentChapterQuery } from '../entities/CurrentChapter';

const HomePage: React.FC = () => {
  const navigate = useNavigate();
  const authorizationData = useGetAuthorizationDataQuery();
  const currentChapterResult = useGetCurrentChapterQuery(undefined, { skip: !authorizationData?.data?.isAuthorized });
  const [autoRegister, autoRegisterResult] = useAutoRegisterMutation();
  const [dropStory, dropStoryResult] = useDropStoryMutation();

  const isLoading = authorizationData.isLoading || autoRegisterResult.isLoading || currentChapterResult.isLoading || dropStoryResult.isLoading;
  const error = authorizationData.error ?? autoRegisterResult.error ?? currentChapterResult.error ?? dropStoryResult.error;
  
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
          Начните своё приключение, {authorizationData.data!.user!.userName}!
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
          Продолжите вашу историю, {authorizationData.data!.user!.userName}!?
        </Typography>
        <ButtonWithLink to={'/game'} text={'Продолжить игру'} />
        <YagoButton onClick={() => sendDropStory()} text={'Удалить сохранения'} isDisabled={false} />
        <ButtonWithLink to={'/registration'} text={'Изменить имя/пароль'} />
      </>
    )
  }

  const renderCard = () => {
    const isAuthorized = authorizationData?.data?.isAuthorized;
    const hasProgress = isAuthorized && (currentChapterResult?.data?.currentSlideIndex ?? 0) > 0

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