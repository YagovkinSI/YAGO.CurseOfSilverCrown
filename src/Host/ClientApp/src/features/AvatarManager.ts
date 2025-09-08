import type { Gender, Race } from "../entities/Character";

const avatars: string[] = [
  'Daji_Female_1',
  'Daji_Female_2',
  'Daji_Male_1',
  'Daji_Male_2',
  
  'Elnir_Female_1',
  'Elnir_Female_2',
  'Elnir_Female_3',
  'Elnir_Male_1',
  'Elnir_Male_2',

  'Isian_Female_1',
  'Isian_Female_2',
  'Isian_Female_3',
  'Isian_Female_4',
  'Isian_Male_1',
  'Isian_Male_2',
  'Isian_Male_3',

  'Khashin_Female_1',
  'Khashin_Female_2',
  'Khashin_Male_1',
  'Khashin_Male_2',

  'Khazadin_Female_1',
  'Khazadin_Female_2',
  'Khazadin_Female_3',
  'Khazadin_Male_1',
  'Khazadin_Male_2',
  'Khazadin_Male_3',
  
  'Nahumi_Female_1',
  'Nahumi_Female_2', 
  'Nahumi_Female_3', 
  'Nahumi_Male_1',
  'Nahumi_Male_2',  
];

const getFallbackAvatar = (race: Race, gender: Gender): string => {
  const fallbacks = avatars.filter(avatar => {
    const [avatarRace, avatarGender] = avatar.split('_');

    const raceFallback = race === 'Unknown' || avatarRace === 'Unknown' || avatarRace === race;
    const genderFallback = gender === 'Unknown' || avatarGender === 'Unknown' || avatarGender === gender;

    return raceFallback && genderFallback;
  });

  if (fallbacks.length > 0) {
    return fallbacks[0];
  }

  return avatars[0];
}

const getMatchingAvatars = (race: Race, gender: Gender, num?: number): string[] => {
  return avatars.filter(avatar => {
    const [avatarRace, avatarGender, avatarNum] = avatar.split('_');

    const raceMatches = race === 'Unknown' || avatarRace === race;

    const genderMatches = gender === 'Unknown' || avatarGender === gender;

    const numMatches = num === undefined || avatarNum === num.toString();

    return raceMatches && genderMatches && numMatches;
  });
}

export const getRandomAvatar = (race: Race, gender: Gender, num?: number): string => {
  const matchingAvatars = getMatchingAvatars(race, gender, num);

  if (matchingAvatars.length === 0) {
    return getFallbackAvatar(race, gender);
  }

  const randomIndex = Math.floor(Math.random() * matchingAvatars.length);
  return matchingAvatars[randomIndex];
}

export const getAvatarCount = (race: Race, gender: Gender): number => {
  return getMatchingAvatars(race, gender).length;
}
