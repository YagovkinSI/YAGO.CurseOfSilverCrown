import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import YagoButton from '../shared/YagoButton';
import { useGetAuthorizationDataQuery } from '../entities/AuthorizationData';
import { useGetCurrentCharacterQuery, useRemoveCharacterMutation } from '../entities/Character';

const HomePage: React.FC = () => {
  const authorizationData = useGetAuthorizationDataQuery();
  const character = useGetCurrentCharacterQuery(undefined, { skip: !authorizationData?.data?.isAuthorized });

  const [removeCharacter, removeCharacterResult] = useRemoveCharacterMutation();

  const isLoading = authorizationData.isLoading || character.isLoading || removeCharacterResult.isLoading;
  const error = authorizationData.error ?? character.error ?? removeCharacterResult.error;

  const sendDropStory = () => {
    removeCharacter({});
  }

  const renderGuestContent = () => {
    return (
      <>
        <Typography textAlign="justify" gutterBottom>
          Пройдите авторизацию или начните игру с временным аккаунтом.
        </Typography>
        <ButtonWithLink to={'/registration'} text={'Авторизация'} />
      </>
    )
  }
  const renderCreateCharacterContent = () => {
    return (
      <>
        <Typography textAlign="justify" gutterBottom>
          Начните своё приключение, {authorizationData.data!.user!.userName}!
        </Typography>
        <ButtonWithLink to={'/createCharacter'} text={'Создать персонажа'} />
        <ButtonWithLink to={'/registration'} text={'Изменить имя/пароль'} />
      </>
    )
  }
  const renderContinueGameContent = () => {
    return (
      <>
        <Typography textAlign="justify" gutterBottom>
          Продолжите вашу историю, {authorizationData.data!.user!.userName}!?
        </Typography>
        <ButtonWithLink to={'/game'} text={'Продолжить игру'} />
        <YagoButton onClick={() => sendDropStory()} text={'Удалить персонажа'} isDisabled={false} />
        <ButtonWithLink to={'/registration'} text={'Изменить имя/пароль'} />
      </>
    )
  }

  const renderCard = () => {
    const isAuthorized = authorizationData?.data?.isAuthorized;
    const hasCharacter = isAuthorized && character?.data

    return (
      <YagoCard
        title={`Yago World`}
        image={'/assets/images/pictures/home.jpg'}
      >
        {isAuthorized
          ? hasCharacter
            ? renderContinueGameContent()
            : renderCreateCharacterContent()
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