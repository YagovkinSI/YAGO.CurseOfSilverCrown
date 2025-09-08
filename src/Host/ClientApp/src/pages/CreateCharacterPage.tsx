import React, { useState } from 'react';
import { Typography, Button, IconButton, Box, CircularProgress, TextField } from '@mui/material';
import { ArrowBack, ArrowForward } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { useCreateCharacterMutation, type Character } from '../entities/Character';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import type { SerializedError } from '@reduxjs/toolkit';
import type { FetchBaseQueryError } from '@reduxjs/toolkit/query';
import YagoCard from '../shared/YagoCard';
import { getAvatarCount, getAvatar } from '../features/AvatarManager';

interface RaceOption {
  value: Character['race'];
  label: string;
  description: string;
  bonus: string
}

interface GenderOption {
  value: Character['gender'];
  label: string;
}

interface BackgroundOption {
  value: Character['background'];
  label: string;
  description: string;
  bonus: string;
}

const CreateCharacterPage: React.FC = () => {
  const navigate = useNavigate();
  const [createCharacter, { isLoading: isSending }] = useCreateCharacterMutation();

  const [error, setError] = useState<FetchBaseQueryError | SerializedError | undefined>(undefined);
  const isLoading = false;

  const [step, setStep] = useState<'race' | 'gender' | 'background' | 'avatar'>('race');
  const [avatarNum, setAvatarNum] = useState(1);
  const [name, setName] = useState('');
  const [nameError, setNameError] = useState('');

  const [characterData, setCharacterData] = useState<Partial<Character>>({
    name: '',
    race: 'Isian',
    gender: 'Male',
    background: 'Mercenary',
    force: 0,
    diplomacy: 0,
    cunning: 0,
    inventory: '',
    avatar: 'Isian_Male_1'
  });

  const avatarCount = step != 'avatar' ? 0 : getAvatarCount(characterData.race!, characterData.gender!);

  const races: RaceOption[] = [
    {
      value: 'Isian',
      label: 'Исей',
      description: 'Жители умеренных земель, известные своим умением находить общий язык с разными культурами. Их адаптивность — главный ключ к влиянию.',
      bonus: '+2 к дипломатии'
    },
    {
      value: 'Nahumi',
      label: 'Нахумец',
      description: 'Уроженцы знойных пустынь и торговых городов-оазисов. Искусные торговцы, чьи улыбки и сделки хранят тысячу секретов.',
      bonus: '+1 к дипломатии, +1 к хитрости'
    },
    {
      value: 'Daji',
      label: 'Даджи',
      description: 'Выходцы из южных саванн и джунглей, чья легендарная физическая мощь и несгибаемый дух рождались в борьбе с суровой природой.',
      bonus: '+2 к силе'
    },
    {
      value: 'Khashin',
      label: 'Хашин',
      description: 'Пришельцы с бескрайних восточных степей. Их выживание зависит от острого ума, предвидения и умения быть невидимым до решающего момента.',
      bonus: '+2 к хитрости'
    },
    {
      value: 'Elnir',
      label: 'Эльнир',
      description: 'Эльниры славятся своей магией, манерами и изяществом. Они предпочитают решать вопросы умом и красноречием.',
      bonus: '-1 к силе, +2 к дипломатии, +1 к хитрости'
    },
    {
      value: 'Khazadin',
      label: 'Хазадин',
      description: 'Короткорукие и несгибаемые обитатели горных твердынь. Их слово — закон, а прямота и честность ценятся выше любого коварства.',
      bonus: '+2 к силе, +1 к дипломатии, -1 к хитрости'
    }
  ];

  const genders: GenderOption[] = [
    { value: 'Male', label: 'Мужской' },
    { value: 'Female', label: 'Женский' }
  ];

  const backgrounds: BackgroundOption[] = [
    {
      value: 'Mercenary',
      label: 'Наёмник',
      description: 'Ваша жизнь — это сталь и договор. Вы уверенно владеете оружием и знаете, как заставить других уважать вашу силу. Вы предпочитаете решать вопросы прямо и решительно, полагаясь на мощь и устрашение.',
      bonus: '+3 к силе'
    },
    {
      value: 'Emissary',
      label: 'Посланник',
      description: 'Ваша валюта — слова и связи. Вы умеете находить подход к самым разным людям, договариваться там, где другие готовы схватиться за мечи, и вас знают в нужных кругах. Ваше оружие — убеждение и дипломатия.',
      bonus: '+3 к дипломатии'
    },
    {
      value: 'Spy',
      label: 'Шпион',
      description: 'Вы действуете из тени. Ваши главные инструменты — хитрость, ловкость и умение оставаться незамеченным. Там, где другие идут в лобовую атаку, вы находите обходной путь, добываете секреты и наносите удар в самый неожиданный момент.',
      bonus: '+3 к хитрости'
    }
  ];

  const handleBack = () => {
    const steps: typeof step[] = ['race', 'gender', 'background', 'avatar'];
    const currentIndex = steps.indexOf(step);
    if (currentIndex > 0) {
      setStep(steps[currentIndex - 1]);
    } else {
      navigate(-1);
    }
  };

  const calcIndex = (currentIndex: number, isNext: boolean, length: number): number => {
    return isNext
      ? (currentIndex + 1) % length
      : (currentIndex - 1 + length) % length;
  }

  const handleSelectRace = (isNext: boolean) => {
    const currentIndex = races.findIndex(r => r.value === characterData.race);
    const newIndex = calcIndex(currentIndex, isNext, races.length);
    setCharacterData({ ...characterData, race: races[newIndex].value });
  };

  const handleSelectGender = (isNext: boolean) => {
    const currentIndex = genders.findIndex(g => g.value === characterData.gender);
    const newIndex = calcIndex(currentIndex, isNext, races.length);
    setCharacterData({ ...characterData, gender: genders[newIndex].value });
  };

  const handleSelectBackground = (isNext: boolean) => {
    const currentIndex = backgrounds.findIndex(b => b.value === characterData.background);
    const newIndex = calcIndex(currentIndex, isNext, races.length);
    setCharacterData({ ...characterData, background: backgrounds[newIndex].value });
  };

  const handleSelectAvatar = (isNext: boolean) => {
    const newIndex = calcIndex(avatarNum - 1, isNext, avatarCount);
    setAvatarNum(newIndex + 1);
  };

  const handleSaveCharacter = async (characterData: Partial<Character>) => {
    try {
      setError(undefined);
      await createCharacter({ character: characterData as Character }).unwrap();
      navigate('/game');
    } catch (error) {
      setError(error as FetchBaseQueryError | SerializedError);
    }
  };

  const renderArrowSelector = (label: string, action: (idNext: boolean) => void) => {
    return (
      <Box display="flex" alignItems="center" justifyContent="center" mb={2}>
        <IconButton onClick={() => action(true)} size="large">
          <ArrowBack />
        </IconButton>
        <Box mx={2} textAlign="center">
          <Typography variant="h6" mt={1}>{label}</Typography>
        </Box>
        <IconButton onClick={() => action(false)} size="large">
          <ArrowForward />
        </IconButton>
      </Box>
    )
  }

  const renderRaceStep = () => {
    const currentRace = races.find(r => r.value === characterData.race)!;

    return (
      <>
        {renderArrowSelector(currentRace.label, handleSelectRace)}
        <Button variant="contained" onClick={() => setStep('gender')}>Выбрать</Button>
        <Typography variant="body2" color="text.secondary">{currentRace.bonus}</Typography>
        <Typography variant="body2" color="text.secondary" mb={2}>{currentRace.description}</Typography>
      </>
    );
  };

  const renderGenderStep = () => {
    const currentGender = genders.find(g => g.value === characterData.gender)!;

    return (
      <>
        {renderArrowSelector(currentGender.label, handleSelectGender)}
        <Button variant="contained" onClick={() => setStep('background')}>Выбрать</Button>
      </>
    );
  };

  const renderBackgroundStep = () => {
    const currentBackground = backgrounds.find(b => b.value === characterData.background)!;

    return (
      <>
        {renderArrowSelector(currentBackground.label, handleSelectBackground)}
        <Button variant="contained" onClick={() => setStep('avatar')}>Выбрать</Button>
        <Typography variant="body2" color="text.secondary">{currentBackground.bonus}</Typography>
        <Typography variant="body2" color="text.secondary" mb={2}>{currentBackground.description}</Typography>
      </>
    );
  };

  const renderAvatarStep = () => {

    const validateName = (value: string): boolean => {
      const regex = /^[a-zA-Zа-яА-Я0-9]{3,16}$/;
      if (!regex.test(value)) {
        setNameError('Имя должно содержать 3-16 символов (буквы и цифры)');
        return false;
      }
      setNameError('');
      return true;
    };

    const handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const value = e.target.value;
      setName(value);
      if (value.length > 0) {
        validateName(value);
      } else {
        setNameError('');
      }
    };

    const handleSave = () => {
      if (validateName(name)) {
        const avatar = getAvatar(
          characterData.race!,
          step === 'race' ? 'Unknown' : characterData.gender!,
          step === 'gender' || step === 'background' ? undefined : avatarNum
        )
        setCharacterData({ ...characterData, name: name, avatar: avatar });
        const characterToSave = { ...characterData, name, avatar: avatar };
        handleSaveCharacter(characterToSave);
      }
    };

    return (
      <>
        {renderArrowSelector('Аватар', handleSelectAvatar)}
        <Box mx={2} textAlign="center">
          <Box mb={2}>
            <TextField
              fullWidth
              label="Имя персонажа"
              value={name}
              onChange={handleNameChange}
              error={!!nameError}
              helperText={nameError}
              inputProps={{
                maxLength: 16,
                pattern: '[a-zA-Zа-яА-Я0-9]{3,16}'
              }}
              sx={{ mb: 2 }}
            />
          </Box>
        </Box>
        <Button
          variant="contained"
          onClick={handleSave}
          disabled={isSending || !name}
        >
          {isSending ? <CircularProgress size={24} /> : 'Сохранить'}
        </Button>
      </>
    );
  };

  const renderCard = () => {
    const title = step === 'race' ? 'Выберите расу'
      : step === 'gender' ? 'Выберите пол'
        : step === 'background' ? 'Выберите предысторию'
          : 'Выберите аватар и имя';

    const avatar = getAvatar(
      characterData.race!,
      step === 'race' ? 'Unknown' : characterData.gender!,
      step != 'avatar' ? undefined : avatarNum
    )

    return (
      <YagoCard
        title={title}
        image={`/assets/images/avatars/${avatar}.jpg`}
        handleBack={handleBack}
      >
        {step === 'race' && renderRaceStep()}
        {step === 'gender' && renderGenderStep()}
        {step === 'background' && renderBackgroundStep()}
        {step === 'avatar' && renderAvatarStep()}
      </YagoCard>
    )
  }

  return (
    <>
      <ErrorField title='Ошибка' error={error} />
      {isLoading
        ? <LoadingCard />
        : error != undefined
          ? <DefaultErrorCard />
          : renderCard()}
    </>
  )
}

export default CreateCharacterPage;