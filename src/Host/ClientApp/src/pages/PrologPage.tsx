import YagoCard from '../shared/YagoCard';
import ButtonWithLink from '../shared/ButtonWithLink';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { Typography } from '@mui/material';
import { useState } from 'react';
import YagoButton from '../shared/YagoButton';

const PrologPage: React.FC = () => {

  interface Slide {
    image: string,
    text: string[]
  }
  
  const slides : Slide[] = [
    {
      image: '/assets/images/pictures/Vulcano.jpg',
      text: [
        'Гнев богов обрушился на Триморье. Вулкан, спавший тысячелетия, разорвал небо огнем и пеплом, смывая волнами великие города эльниров с их дворцами и гаванями. Ядовитый туман окутал побережье, порты замерли, а солнце скрылось за серной пеленой. Началась эпоха, что войдёт в историю как «Пепел Власти».'
      ]
    },
    {
      image: '/assets/images/pictures/Palace.jpg',
      text: [
        'Вы — наместник одного из полисов Триморья. Вы управляли городскими делами, пока ваш покровитель, архонт-эльнир Теларион, вёл переговоры на островах. Теперь вести с моря оборвались. Лишь шепот рыбаков доносит слухи о том, что его великую трирему поглотила разъярённая стихия где-то в открытом море.'
      ]
    },
    {
      image: '/assets/images/pictures/WorriedPeople.jpg',
      text: [
        'Ваш город на грани хаоса. Портовый район повреждён волнами, с моря прибывают толпы испуганных беженцев, а местная знать уже шепчется в своих покоях о смещении наместника и смене власти.',
      ]
    },
    {
      image: '/assets/images/pictures/homepage.jpg',
      text: [
        'На ваши плечи легла тяжёлая ноша. От ваших решений зависит, возродится ли город из пепла, превратится ли в могущественный оплот или падёт, став лишь ещё одной забытой страницей в летописи нового времени. Началась эпоха «Пепла Власти». И вы — один из тех, кто будет вершить её историю.'
      ]
    },
  ] 

  const [currentIndex, setCurrentIndex] = useState<number>(0);

  const isLoading = false;
  const error = undefined;

  const renderCard = () => {
    const isLastCard = slides.length == currentIndex + 1;
    const hasBack = currentIndex > 0;
    const hasContinue = !isLastCard;

    const slide = slides[currentIndex];

    return (
      <YagoCard
        title={'Пролог'}
        image={slide.image}
        headerButtonsAccess={false}
      >
        {slide.text.map(t => 
          <Typography textAlign="justify" gutterBottom>
            {t}
          </Typography>
        )}
        {isLastCard && <ButtonWithLink to={'/app/map'} text={'Выбрать владение на карте'} />}
        {hasBack && <YagoButton onClick={() => setCurrentIndex(currentIndex - 1)} text={'Назад'} isDisabled={false} />}
        {hasContinue && <YagoButton onClick={() => setCurrentIndex(currentIndex + 1)} text={'Далее'} isDisabled={false} />}
      </YagoCard>
    )
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : error == undefined
          ? renderCard()
          : <DefaultErrorCard />}
    </>
  )
}

export default PrologPage