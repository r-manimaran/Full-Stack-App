import React from 'react';
import { Box, Avatar } from '@mui/material';

interface StationLogoProps {
  station: string;
  size?: number;
}

const StationLogo: React.FC<StationLogoProps> = ({ station, size = 32 }) => {
  // Station logo mapping - can be extended with actual image URLs later
  const getStationLogo = (stationName: string): string | null => {
    const logoMap: Record<string, string> = {
      'BJs': '🏪',
      'Costco': '🏬',
      'Circle K': '⭕',
      'Mobil': '🔴',
      'Shell': '🟡',
      'BP': '🟢',
      'Chevron': '🔵',
      'Exxon': '🔴',
      'Other': '⛽',
    };
    return logoMap[stationName] || '⛽';
  };

  const logo = getStationLogo(station);

  return (
    <Avatar
      sx={{
        width: size,
        height: size,
        bgcolor: 'primary.light',
        fontSize: size * 0.6,
      }}
    >
      {logo}
    </Avatar>
  );
};

export default StationLogo;
