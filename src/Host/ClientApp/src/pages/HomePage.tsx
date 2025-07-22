import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import YagoButton from '../shared/YagoButton';
import { useGetCurrentUserQuery, useDropStoryMutation } from '../entities/ApiEndpoints';

const HomePage: React.FC = () => {
  const currentUserResult = useGetCurrentUserQuery();
  const [dropStory, dropStoryResult] = useDropStoryMutation();

  const isLoading = currentUserResult.isLoading || dropStoryResult.isLoading;
  const error = currentUserResult.error ?? dropStoryResult.error;

  const sendDropStory = () => {
    dropStory({});
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
    const hasProgress = (dropStoryResult?.data?.id ?? currentUserResult?.data?.storyNode?.id ?? 0) > 0

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