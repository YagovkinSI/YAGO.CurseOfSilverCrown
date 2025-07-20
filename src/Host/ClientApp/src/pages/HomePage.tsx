import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import { useGetCurrentUserQuery } from '../entities/CurrentUser';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { useDropStoryMutation, useGetCurrentStoryQuery } from '../entities/StoryNode';
import { Typography } from '@mui/material';
import YagoButton from '../shared/YagoButton';

const HomePage: React.FC = () => {
  const currentUserResult = useGetCurrentUserQuery();
  const currentStoryResult = useGetCurrentStoryQuery();
  const [dropStory] = useDropStoryMutation();

  const isLoading = currentUserResult.isLoading || currentStoryResult.isLoading;
  const error = currentUserResult.error ?? (currentUserResult.data?.isAuthorized ? currentStoryResult.error : undefined);

  const sendDropStory = () => {
    currentStoryResult.isLoading = true;
    dropStory();
  }

  const renderGuestContent = () => {
    return (
      <>
        <Typography textAlign="justify" gutterBottom>
          Войдите, чтобы начать свою историю в этом мире.
        </Typography>
        <ButtonWithLink to={'/registration'} text={'Авторизация'} />
        <ButtonWithLink to={'/game'} text={'Игра'} />
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
    const hasProgress = (currentStoryResult?.data?.id ?? 0) > 0

    return (
      <YagoCard
        title={`Yago World`}
        image={'/assets/images/pictures/home.jpg'}
      >
        {isAuthorized
          ? hasProgress
            ? renderContinueStoryContent()
            : renderNewStoryContent()
          : renderGuestContent() }
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