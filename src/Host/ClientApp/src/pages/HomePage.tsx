import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import { useGetCurrentUserQuery } from '../entities/CurrentUser';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';

const HomePage: React.FC = () => {
  const { data, isLoading, error } = useGetCurrentUserQuery();

  const renderCard = () => {
    const isAuthorized = data?.isAuthorized;

    return (
      <YagoCard
        title={`Новый дом`}
        image={'/assets/images/pictures/home.jpg'}
      >
        <Typography textAlign="justify" gutterBottom>
          Мальчик стоял у окна, вглядываясь в серые улицы Истиллы. Он не знал, каким был этот город до извержения - возможно, таким же прекрасным, как Жемчужная Гавань, где воздух дрожал от морского бриза. Но теперь здесь пахло только камнем и гарью. Однако больше всего мальчика поражало обилие богатых людей - почти как эльнирских господ в его родном городе. Но в отличие от Жемчужной Гавани, здесь люди спорили с эльнирами на равных, не склоняя голов.
        </Typography>
        {isAuthorized
          ? <ButtonWithLink to={'/app/game'} text={'Продолжить игру'} />
          : <ButtonWithLink to={'/app/registration'} text={'Авторизация'} />}
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