import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import YagoButton from '../shared/YagoButton';
import { useNavigate } from 'react-router-dom';
import { useCreateTemporaryUserMutation, useGetMyUserQuery } from '../entities/MyUser';
import TextFooterComment from '../shared/TextFooterComment';
import { useGetMyColonyQuery } from '../entities/MyColony';

const HomePage: React.FC = () => {
  const myUserDataResult = useGetMyUserQuery();
  const myColonyResult = useGetMyColonyQuery();
  const [createTemporaryUser, createTemporaryUserResult] = useCreateTemporaryUserMutation();
  const navigate = useNavigate();

  const isLoading = myUserDataResult.isLoading || myColonyResult.isLoading || createTemporaryUserResult.isLoading;
  const error = myUserDataResult.error ?? myColonyResult.error ?? createTemporaryUserResult.error;
  const user = myUserDataResult.data?.data;
  const colony = myColonyResult.data?.data;

  const autoRegisterAndGame = () => {
    createTemporaryUser({})
      .unwrap()
      .then(() => navigate('/createColony'));
  }

  const renderGuestContent = () => {
    return (
      <>
        <YagoButton onClick={autoRegisterAndGame} text={'Быстрый старт'} isDisabled={false} />
        <ButtonWithLink to={'/registration'} text={'Войти / Регистрация'} />
      </>
    )
  }

  const renderAuthorizedUserContent = () => {
    const buttonName = colony == undefined
      ? 'Создать колонию'
      : user!.isTemporary
        ? `Продолжить как ${user?.userName}`
        : `В колонию ${colony.name}`

    return (
      <>
        {user!.isTemporary && <ButtonWithLink to={'/registration'} text={'Изменить имя и пароль'} />}
        <ButtonWithLink to={'/me/colony'} text={buttonName} />
      </>
    )
  }

  const renderCard = () => {
    const isAuthorized = myUserDataResult.data?.data != undefined;

    return (
      <YagoCard
        title={`Мир YAGO`}
        image={'/assets/images/pictures/homepage.jpg'}
        headerButtonsAccess={false}
      >
        <Typography textAlign="center" gutterBottom>
          Каким будет твоё государство среди звёзд?
        </Typography>
        {isAuthorized
          ? renderAuthorizedUserContent()
          : renderGuestContent()}
        <TextFooterComment>
          Для создания визуального и текстового контента в этой игре в качестве инструмента прототипирования и вдохновения использовались технологии искусственного интеллекта. Финальный творческий отбор и интеграция выполнены разработчиком. Мы с уважением относимся к творчеству художников и писателей по всему миру.
        </TextFooterComment>
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