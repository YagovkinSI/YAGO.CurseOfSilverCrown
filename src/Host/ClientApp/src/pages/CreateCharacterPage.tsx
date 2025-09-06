import React, { useState } from 'react';
import { CardContent, Typography, Button, IconButton, Box, Grid, Avatar, CircularProgress, TextField } from '@mui/material';
import { ArrowBack, ArrowForward } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { useCreateCharacterMutation, type Character } from '../entities/Character';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import type { SerializedError } from '@reduxjs/toolkit';
import type { FetchBaseQueryError } from '@reduxjs/toolkit/query';
import YagoCard from '../shared/YagoCard';

interface RaceOption {
  value: Character['race'];
  label: string;
  image: string;
  description: string;
}

interface GenderOption {
  value: Character['gender'];
  label: string;
}

interface BackgroundOption {
  value: Character['background'];
  label: string;
  description: string;
}

const CreateCharacterPage: React.FC = () => {
  const navigate = useNavigate();
  const [createCharacter, { isLoading: isSending }] = useCreateCharacterMutation();

  const error: FetchBaseQueryError | SerializedError | undefined = undefined;
  const isLoading = false;

  const [currentStepName, setCurrentStepName] = useState<string>('Персонаж');
  const [currentImage, setCurrentImage] = useState<string>('home');
  const [step, setStep] = useState<'race' | 'gender' | 'background' | 'avatar'>('race');
  const [name, setName] = useState('');
  const [nameError, setNameError] = useState('');

  const [characterData, setCharacterData] = useState<Partial<Character>>({
    race: 'Isian',
    gender: 'Male',
    background: 'Mercenary',
    force: 5,
    diplomacy: 5,
    cunning: 5,
    inventory: '',
    image: '/avatars/1.jpg'
  });

  const races: RaceOption[] = [
    {
      value: 'Isian',
      label: 'Исианец',
      image: '/races/isian.jpg',
      description: 'Технологически продвинутая раса с высоким интеллектом'
    },
    {
      value: 'Nahumi',
      label: 'Нахуми',
      image: '/races/nahumi.jpg',
      description: 'Мирные земледельцы с сильной связью с природой'
    },
    {
      value: 'Daji',
      label: 'Даджи',
      image: '/races/daji.jpg',
      description: 'Воинственная раса с развитой мышечной массой'
    },
    {
      value: 'Khashin',
      label: 'Хашин',
      image: '/races/khashin.jpg',
      description: 'Торговцы и дипломаты с врожденной харизмой'
    },
    {
      value: 'Elnir',
      label: 'Эльнир',
      image: '/races/elnir.jpg',
      description: 'Мистическая раса с развитой интуицией'
    },
    {
      value: 'Khazadin',
      label: 'Хазадин',
      image: '/races/khazadin.jpg',
      description: 'Подземные жители с исключительной выносливостью'
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
      description: '+2 к силе, бонус к бою'
    },
    {
      value: 'Emissary',
      label: 'Посланник',
      description: '+2 к дипломатии, бонус к переговорам'
    },
    {
      value: 'Spy',
      label: 'Шпион',
      description: '+2 к хитрости, бонус к скрытности'
    }
  ];

  const avatars = [
    '/avatars/1.jpg',
    '/avatars/2.jpg',
    '/avatars/3.jpg',
    '/avatars/4.jpg',
    '/avatars/5.jpg',
    '/avatars/6.jpg'
  ];

  const getCurrentRaceIndex = () =>
    races.findIndex(r => r.value === characterData.race);

  const getCurrentGenderIndex = () =>
    genders.findIndex(g => g.value === characterData.gender);

  const getCurrentBackgroundIndex = () =>
    backgrounds.findIndex(b => b.value === characterData.background);

  const getCurrentAvatarIndex = () =>
    avatars.findIndex(b => b === characterData.background);

  const handleNextRace = () => {
    const currentIndex = getCurrentRaceIndex();
    const nextIndex = (currentIndex + 1) % races.length;
    setCharacterData({ ...characterData, race: races[nextIndex].value });
  };

  const handlePrevRace = () => {
    const currentIndex = getCurrentRaceIndex();
    const prevIndex = (currentIndex - 1 + races.length) % races.length;
    setCharacterData({ ...characterData, race: races[prevIndex].value });
  };

  const handleNextGender = () => {
    const currentIndex = getCurrentGenderIndex();
    const nextIndex = (currentIndex + 1) % genders.length;
    setCharacterData({ ...characterData, gender: genders[nextIndex].value });
  };

  const handlePrevGender = () => {
    const currentIndex = getCurrentGenderIndex();
    const prevIndex = (currentIndex - 1 + genders.length) % genders.length;
    setCharacterData({ ...characterData, gender: genders[prevIndex].value });
  };

  const handleNextBackground = () => {
    const currentIndex = getCurrentBackgroundIndex();
    const nextIndex = (currentIndex + 1) % backgrounds.length;
    setCharacterData({ ...characterData, background: backgrounds[nextIndex].value });
  };

  const handlePrevBackground = () => {
    const currentIndex = getCurrentBackgroundIndex();
    const prevIndex = (currentIndex - 1 + backgrounds.length) % backgrounds.length;
    setCharacterData({ ...characterData, background: backgrounds[prevIndex].value });
  };

  const handleNextAvatar = () => {
    const currentIndex = getCurrentAvatarIndex();
    const nextIndex = (currentIndex + 1) % avatars.length;
    setCharacterData({ ...characterData, image: avatars[nextIndex] });
  };

  const handlePrevAvatar = () => {
    const currentIndex = getCurrentAvatarIndex();
    const prevIndex = (currentIndex - 1 + avatars.length) % backgrounds.length;
    setCharacterData({ ...characterData, image: avatars[prevIndex] });
  };

  const handleSaveCharacter = async () => {
    try {
      await createCharacter({ character: characterData as Character }).unwrap();
      navigate('/game');
    } catch (error) {
      console.error('Failed to create character:', error);
    }
  };

  const renderRaceStep = () => {
    const currentRace = races.find(r => r.value === characterData.race)!;

    return (
      <>
        <Box display="flex" alignItems="center" justifyContent="center" mb={2}>
          <IconButton onClick={handlePrevRace} size="large">
            <ArrowBack />
          </IconButton>
          <Box mx={2} textAlign="center">
            <Typography variant="h6" mt={1}>{currentRace.label}</Typography>
          </Box>
          <IconButton onClick={handleNextRace} size="large">
            <ArrowForward />
          </IconButton>
        </Box>
        <Typography variant="body2" color="text.secondary" mb={2}>
          {currentRace.description}
        </Typography>
        <Button variant="contained" onClick={() => setStep('gender')}>
          Выбрать
        </Button>
      </>
    );
  };

  const renderGenderStep = () => {
    const currentGender = genders.find(g => g.value === characterData.gender)!;

    return (
      <>
        <Box display="flex" alignItems="center" justifyContent="center" mb={2}>
          <IconButton onClick={handlePrevGender} size="large">
            <ArrowBack />
          </IconButton>
          <Box mx={2} textAlign="center">
            <Typography variant="h6" mt={1}>{currentGender.label}</Typography>
          </Box>
          <IconButton onClick={handleNextGender} size="large">
            <ArrowForward />
          </IconButton>
        </Box>
        <Button variant="contained" onClick={() => setStep('background')}>
          Выбрать
        </Button>
      </>
    );
  };

  const renderBackgroundStep = () => {
    const currentBackground = backgrounds.find(b => b.value === characterData.background)!;

    return (
      <>
        <Box display="flex" alignItems="center" justifyContent="center" mb={2}>
          <IconButton onClick={handlePrevBackground} size="large">
            <ArrowBack />
          </IconButton>
          <Box mx={2} textAlign="center">
            <Typography variant="h6">{currentBackground.label}</Typography>
          </Box>
          <IconButton onClick={handleNextBackground} size="large">
            <ArrowForward />
          </IconButton>
        </Box>
        <Button variant="contained" onClick={() => setStep('avatar')}>
          Выбрать
        </Button>
        <Typography variant="body2" color="text.secondary">
          {currentBackground.description}
        </Typography>
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
        setCharacterData({ ...characterData, name, image: characterData.image });
        handleSaveCharacter();
      }
    };

    return (
      <>
        <Box display="flex" alignItems="center" justifyContent="center" mb={2}>
          <IconButton onClick={handlePrevAvatar} size="large">
            <ArrowBack />
          </IconButton>
          <Box mx={2} textAlign="center">
            <Typography variant="h6">Аватар</Typography>
          </Box>
          <IconButton onClick={handleNextAvatar} size="large">
            <ArrowForward />
          </IconButton>
        </Box>
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
          disabled={isSending || !characterData.image || !name}
        >
          {isSending ? <CircularProgress size={24} /> : 'Сохранить'}
        </Button>
      </>
    );
  };

  const renderCard = () => {
    return (
      <YagoCard
        title={currentStepName}
        image={`/assets/images/pictures/${currentImage ?? 'home'}.jpg`}
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